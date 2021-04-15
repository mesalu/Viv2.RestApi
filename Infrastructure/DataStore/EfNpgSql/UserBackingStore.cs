using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Services;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql
{
    public class UserBackingStore : IUserBackingStore
    {
        private readonly UserManager<BackedUser> _userManager;

        public UserBackingStore(UserManager<BackedUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<User> GetUserByName(string name) => (await _userManager.FindByNameAsync(name)).ToCoreUser();

        public async Task<User> GetUserById(Guid id) => (await _userManager.FindByIdAsync(id.ToString())).ToCoreUser();

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
            throw new System.NotImplementedException();
        }
    }
}
