using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;
using Environment = Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class UserStore : IUserStore
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public UserStore(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        
        public async Task<IUser> GetUserByName(string name) => await _userManager.FindByNameAsync(name);

        public async Task<IUser> GetUserById(Guid id) => await _userManager.FindByIdAsync(id.ToString());

        public async Task<Guid> CreateUser(IUser user, string password)
        {
            // Don't need to use the full `applycoreuser` method, as most fields will be empty
            // also, copying the Guid around can be detrimental at this phase.
            User backableUser = new User
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
                    return backableUser.Guid;
                }
            }
            catch (Exception)
            {
                throw new Core.Exceptions.UserStoreException("Failed to create new user");
            }

            return Guid.Empty;
        }

        public async Task<bool> CheckPassword(IUser user, string password)
        {
            // Well this is an unfortunate side effect of abstraction, and there may be a C#-ism to 
            // mitigate it that I'm not familiar with (maybe partial classes?) Since BackedUser can't directly
            // inherit from Core.Entities.User, we have a conversion method, but that doesn't let us treat
            // Core.Entity.User method as a BackedUser, so we must convert back via hitting the data store. :\
            // TODO: see if just pulling user ID from `user` works for CheckPasswordAsync.
            var storedUser = user as User;
            return await _userManager.CheckPasswordAsync(storedUser, password);
        }

        public async Task PutUser(IUser user)
        {
            // just to keep warnings down (for now)
            var unused = await Task.FromResult(false);
            throw new System.NotImplementedException();
        }

        public async Task UpdateUser([NotNull] IUser user)
        {
            // attempt a type-safe downcast.
            var backedUser = (user as User);
            if (backedUser == null) throw new ArgumentException("Cannot update implementation of IUser");
            await _userManager.UpdateAsync(backedUser);
        }

        public async Task<IEnumerable<string>> GetRoles(IUser user)
        {
            // prefer fetching user from user manager as it may be tracked.
            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            return await _userManager.GetRolesAsync(backedUser);
        }

        public async Task AddToRoles(IUser user, IEnumerable<string> rolesToAdd)
        {
            // not fond of doing this per operation, but it is what it is.
            var asList = rolesToAdd.ToList(); // prevent multiple-enumeration issues.
            foreach (var role in asList) await _CreateRoleIfMissing(role);
            
            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            await _userManager.AddToRolesAsync(backedUser, asList);
        }

        public async Task RemoveRolesFromUser(IUser user)
        {
            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            var roles = await _userManager.GetRolesAsync(backedUser);
            await _userManager.RemoveFromRolesAsync(backedUser, roles);
        }

        public async Task<ICollection<IEnvironment>> LoadEnvironments(IUser user, bool force = false)
        {
            // if we can shortcut:
            if (!force && user.Environments.Count > 0) return user.Environments;

            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            await _context.Entry(backedUser).Collection(u => u.BackedEnvironments).LoadAsync();

            return user.Environments;
        }

        public async Task<ICollection<RefreshToken>> LoadRefreshTokens([NotNull] IUser user, bool force = false)
        {
            if (!force && user.RefreshTokens.Count > 0) return user.RefreshTokens;

            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            await _context.Entry(backedUser).Collection(u => u.RefreshTokens).LoadAsync();
            return user.RefreshTokens;
        }

        public async Task<ICollection<IPet>> LoadPets([NotNull] IUser user, bool force = false)
        {
            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");

            var collection = _context.Entry(backedUser).Collection(u => u.BackedPets);
            
            if (!force && collection.IsLoaded) return user.Pets;

            await collection.LoadAsync();
            return user.Pets;
        }

        public async Task<ICollection<IController>> LoadControllers([NotNull] IUser user, bool force = false)
        {
            var backedUser = user as User;
            if (backedUser == null) throw new ArgumentException("Datastore implementation mismatch.");
            
            // While there is some finickiness with loading environments & controllers,
            // from the perspective of the userStore instance, we can always load
            // all envs associated to a controller - IOW: a user that owns a controller
            // can see all envs on that controller.
            // (Note: a user that can see an env on a controller may not see all envs on
            // said controller)
            
            // break the mold a bit - we need to include a nested collection too.
            if (!force && user.Controllers.Count > 0) return user.Controllers;

            var collection = await _context.Controllers
                .Where(c => c.RealOwner == backedUser)
                .Include(c => c.BackedEnvironments)
                .ToListAsync();

            backedUser.BackedControllers = collection;
            
            return user.Controllers;
        }

        public async Task AddAssociationToEnv([NotNull] IUser user, [NotNull] IEnvironment environment)
        {
            if (!(user is User && environment is Environment)) 
                throw new ArgumentException("Mismatched Datastore implementations");

            // can direct cast here, as we know the type is safe.
            var concreteUser = (User) user;
            var concreteEnv = (Environment) environment;
            
            // find a collection that's already loaded (avoid extra leg work)
            if (_context.Entry(concreteUser).Collection(u => u.BackedEnvironments).IsLoaded)
                concreteUser.BackedEnvironments.Add(concreteEnv);
            
            // else, lets check if its loaded for the environment
            else if (_context.Entry(concreteEnv).Collection(e => e.BackedUsers).IsLoaded)
                concreteEnv.BackedUsers.Add(concreteUser);
            else
            {
                // neither loaded, load one (lets prefer the user nav property for no reason)
                await _context.Entry(concreteUser).Collection(u => u.BackedEnvironments).LoadAsync();
                concreteUser.BackedEnvironments.Add(concreteEnv);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddAssociationToController([NotNull] IUser user, [NotNull] IController controller)
        {
            // ensure correct concrete types.
            var concreteUser = user as User;
            var concreteController = controller as Controller;

            if (concreteController == null || concreteUser == null)
                throw new ArgumentException("Infrastructure type mismatch");

            concreteController.RealOwner = concreteUser;
            if (await _context.Controllers.ContainsAsync(concreteController))
                _context.Controllers.Update(concreteController);
            else
                await _context.Controllers.AddAsync(concreteController);
            
            
            // commit to database.
            await _context.SaveChangesAsync();
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
