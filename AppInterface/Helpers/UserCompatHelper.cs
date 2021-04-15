using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Viv2.API.Core.Constants;

namespace Viv2.API.AppInterface.Helpers
{
    public class UserCompatHelper
    {
        /// <summary>
        /// If the givent context is from an action method that requires authentication, then this method
        /// extracts the GUID of the authorized user and returns it. If there is no retrievable user Id, than
        /// Guid.Empty is returned instead.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Guid UserGuidFromAuthenticatedContext(HttpContext context)
        {
            Claim identityClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimNames.UserId);
            return identityClaim != null ? Guid.Parse(identityClaim.Value) : Guid.Empty;
        }
    }
}
