using System;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Viv2.API.Core.Adapters;

namespace Viv2.API.Infrastructure.JwtMinting
{
    public class ClaimsIdentityCompat : IClaimIdentityCompat
    {
        public Guid ExtractFirstIdClaim(ClaimsPrincipal principal)
        {
            var claimedId = principal.FindFirst(JwtRegisteredClaimNames.Aud)?.Value;
            return (claimedId != null) ? Guid.Parse(claimedId) : Guid.Empty;
        }
    }
}