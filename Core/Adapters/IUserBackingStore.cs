using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Adapters
{
    public interface IUserBackingStore
    {
        Task<IUser> GetUserByName(string name);

        Task<IUser> GetUserById(Guid id);

        /// <summary>
        /// Create a new user and persist it.
        /// </summary>
        /// <param name="user">intended user information</param>
        /// <param name="password">expected password.</param>
        /// <returns>The Guid of the newly created user.</returns>
        /// <exception cref=""></exception>
        Task<Guid> CreateUser(IUser user, string password);
        
        /// <summary>
        /// Given a valid user entity and a possible password, verifies if the password
        /// is correct and returns accordingly.
        /// </summary>
        /// <param name="user">A valid User entity instance.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if `password` correctly corresponds to `user`, false otherwise.</returns>
        Task<bool> CheckPassword(IUser user, string password);

        Task PutUser(IUser user);
        Task UpdateUser(IUser user);

        /// <summary>
        /// Fetches an enumerable containing the proper-roles granted to a user.
        ///
        /// Note: Present implementations bastardizes access-level with roles (we piggy back Bot vs User access
        ///         on top of roles.) While `user` is a proper role, `bot` is not. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>an enumerable containing roles assigned to the user</returns>
        Task<IEnumerable<string>> GetRoles(IUser user);
        
        /// <summary>
        /// Iterates over `rolesToAdd`, adding them to `user` if that role is not already granted.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesToAdd"></param>
        /// <returns></returns>
        Task AddToRoles(IUser user, IEnumerable<string> rolesToAdd);

        /// <summary>
        /// Strips the stored user of all active roles. Will likely be followed by a call to `AddRoles`
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task RemoveRolesFromUser(IUser user);

        /// <summary>
        /// Backing implementations need not ensure that reference properties are loaded eagerly.
        /// This method can be used to ensure that the Environments property is loaded - and back-assigned
        /// to the user instance.
        ///
        /// Accessing properties of the objects in the returned collection may require similar calls to
        /// IEnvironmentDataStore
        /// </summary>
        /// <param name="user">User to load environment data from.</param>
        /// <param name="force">Force a reload if data already present in user</param>
        /// <returns></returns>
        Task<ICollection<IEnvironment>> LoadEnvironments([NotNull] IUser user, bool force = false);

        /// <summary>
        /// Backing implementations need not ensure that reference properties are loaded eagerly.
        /// This method can be used to ensure that the RefreshTokens property is loaded - and back-assigned
        /// to the user instance.
        /// </summary>
        Task<ICollection<RefreshToken>> LoadRefreshTokens([NotNull] IUser user, bool force = false);

        /// <summary>
        /// Backing implementations need not ensure that reference properties are loaded eagerly.
        /// This method can be used to ensure that the Pets property is loaded - and back-assigned
        /// to the user instance.
        /// </summary>
        Task<ICollection<IPet>> LoadPets([NotNull] IUser user, bool force = false);
        
        /// <summary>
        /// Persists an association between user and environment.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task AddAssociationToEnv([NotNull] IUser user, [NotNull] IEnvironment environment);
    }
}
