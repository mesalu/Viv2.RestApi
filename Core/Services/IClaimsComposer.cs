using System.Security.Claims;
using Viv2.API.Core.Entities;

namespace Viv2.API.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClaimsComposer
    {
        /// <summary>
        ///  Composes a claims identity for the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ClaimsIdentity ComposeIdentity(User user);
    }
}
