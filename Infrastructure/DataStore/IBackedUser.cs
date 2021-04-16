using Viv2.API.Core.Entities;

namespace Viv2.API.Infrastructure.DataStore
{
    public interface IBackedUser
    {
        /// <summary>
        /// Converts the infrastructure-specific user object into one Core can recognize and use.
        /// </summary>
        /// <returns></returns>
        User ToCoreUser();

        /// <summary>
        /// Takes the values contained in User and applies it to the IBackedUser instance.
        /// Can be treated as a sort of inverse operation for `ToCoreUser`.
        /// </summary>
        /// <param name="user"></param>
        void ApplyCoreUser(User user);
    }
}
