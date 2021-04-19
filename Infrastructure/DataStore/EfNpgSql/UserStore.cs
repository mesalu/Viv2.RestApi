using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Services;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class UserStore : IUserBackingStore
    {
        private readonly UserManager<BackedUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public UserStore(UserManager<BackedUser> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        
        public async Task<User> GetUserByName(string name) => (await _userManager.FindByNameAsync(name))?.ToCoreUser();

        public async Task<User> GetUserById(Guid id) => (await _userManager.FindByIdAsync(id.ToString()))?.ToCoreUser();

        public async Task<Guid> CreateUser(User user, string password)
        {
            // Don't need to use the full `applycoreuser` method, as most fields will be empty
            // also, copying the Id around can be detrimental at this phase.
            BackedUser backableUser = new BackedUser
            {
                UserName = user.Name,
                Email = user.Email,
            };
            
            try
            {
                IdentityResult result = await _userManager.CreateAsync(backableUser, password);
                if (result.Succeeded)
                {
                    // _userManager doesn't seem to back-assign fields filled during creation, so to get
                    // those (namely, the ID), we'll need to load from store... wooo.
                    backableUser = await _userManager.FindByNameAsync(backableUser.UserName);
                    return Guid.Parse(backableUser.Id);
                }
            }
            catch (Exception)
            {
                throw new Core.Exceptions.UserStoreException("Failed to create new user");
            }

            return Guid.Empty;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            // Well this is an unfortunate side effect of abstraction, and there may be a C#-ism to 
            // mitigate it that I'm not familiar with (maybe partial classes?) Since BackedUser can't directly
            // inherit from Core.Entities.User, we have a conversion method, but that doesn't let us treat
            // Core.Entity.User method as a BackedUser, so we must convert back via hitting the data store. :\
            // TODO: see if just pulling user ID from `user` works for CheckPasswordAsync.
            BackedUser storedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return await _userManager.CheckPasswordAsync(storedUser, password);
        }

        public async Task PutUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateUser(User user)
        {
            var backedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            backedUser.ApplyCoreUser(user);
            await _userManager.UpdateAsync(backedUser);
        }

        public async Task<IEnumerable<string>> GetRoles(User user)
        {
            // prefer fetching user from user manager as it may be tracked.
            var backedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            return await _userManager.GetRolesAsync(backedUser);
        }

        public async Task AddToRoles(User user, IEnumerable<string> rolesToAdd)
        {
            // not fond of doing this per operation, but it is what it is.
            var asList = rolesToAdd.ToList(); // prevent multiple-enumeration issues.
            foreach (var role in asList) await _CreateRoleIfMissing(role);
            
            // prefer fetching user from user manager as it may be tracked at this point.
            var backedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            await _userManager.AddToRolesAsync(backedUser, asList);
        }

        public async Task RemoveRolesFromUser(User user)
        {
            var backedUser = await _userManager.FindByIdAsync(user.Id.ToString());

            var roles = await _userManager.GetRolesAsync(backedUser);
            await _userManager.RemoveFromRolesAsync(backedUser, roles);
        }

        /// <summary>
        /// Creates a role via a RoleManager instance, should that role be missing.
        /// </summary>
        /// <param name="role"></param>
        private async Task _CreateRoleIfMissing(string role)
        {
            if ((await _roleManager.FindByNameAsync(role)) == null)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
