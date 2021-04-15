using System.Security.Claims;
using System.Security.Principal;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Services;
using Viv2.API.Core.Constants;

namespace Viv2.API.Infrastructure.MsIdent
{
    /// <summary>
    /// Implementation of IClaimsComposer specific(ish) to MS Identity framework.
    /// </summary>
    public class ClaimsComposer : IClaimsComposer
    {
        public ClaimsIdentity ComposeIdentity(User user)
        {
            return new (
                new GenericIdentity(user.Name),
                new[]
                {
                    new Claim(ClaimNames.UserId, user.Id.ToString()),
                });
        }
    }
}
