using System.Collections.Generic;
using System.Linq;
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
        public ClaimsIdentity ComposeIdentity(User user, IEnumerable<Claim> extras = null)
        {
            var claims = (extras == null) ? new List<Claim>() : extras.ToList();
            claims.Add(new Claim(ClaimNames.UserId, user.Id.ToString()));
            
            return new ClaimsIdentity(new GenericIdentity(user.Name), claims);
        }
    }
}
