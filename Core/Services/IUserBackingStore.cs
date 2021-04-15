using System;
using System.Threading.Tasks;
using Viv2.API.Core.Entities;

namespace Viv2.API.Core.Services
{
    public interface IUserBackingStore
    {
        Task<User> GetUserByName(string name);

        Task<User> GetUserById(Guid id);
        
        /// <summary>
        /// Given a valid user entity and a possible password, verifies if the password
        /// is correct and returns accordingly.
        /// </summary>
        /// <param name="user">A valid User entity instance.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if `password` correctly corresponds to `user`, false otherwise.</returns>
        Task<bool> CheckPassword(User user, string password);

        Task PutUser(User user);
        Task UpdateUser(User user);
    }
}
