using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viv2.API.Core.Entities;

namespace Viv2.API.Core.Services
{
    public interface IUserBackingStore
    {
        Task<User> GetUserByName(string name);

        Task<User> GetUserById(Guid id);

        /// <summary>
        /// Create a new user and persist it.
        /// </summary>
        /// <param name="user">intended user information</param>
        /// <param name="password">expected password.</param>
        /// <returns>The Guid of the newly created user.</returns>
        /// <exception cref=""></exception>
        Task<Guid> CreateUser(User user, string password);
        
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

        /// <summary>
        /// Fetches an enumerable containing the proper-roles granted to a user.
        ///
        /// Note: Present implementations bastardizes access-level with roles (we piggy back Bot vs User access
        ///         on top of roles.) While `user` is a proper role, `bot` is not. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>an enumerable containing roles assigned to the user</returns>
        Task<IEnumerable<string>> GetRoles(User user);
        
        /// <summary>
        /// Iterates over `rolesToAdd`, adding them to `user` if that role is not already granted.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesToAdd"></param>
        /// <returns></returns>
        Task AddToRoles(User user, IEnumerable<string> rolesToAdd);

        /// <summary>
        /// Strips the stored user of all active roles. Will likely be followed by a call to `AddRoles`
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task RemoveRolesFromUser(User user);
    }
}
