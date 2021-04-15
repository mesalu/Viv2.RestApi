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
    }
}
