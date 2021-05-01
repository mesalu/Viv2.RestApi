using System.Collections.Generic;
using System.Security.Claims;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Adapters
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
        ClaimsIdentity ComposeIdentity(IUser user, IEnumerable<Claim> extras = null);
    }
}
