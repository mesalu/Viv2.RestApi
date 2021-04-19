using System;
using System.Security.Claims;

namespace Viv2.API.Core.Services
{
    /// <summary>
    /// Represents a compatibility layer to smooth out any external-facing specifics a
    /// claims-based authentication scheme may have.
    /// </summary>
    public interface IClaimIdentityCompat
    {
        /// <summary>
        /// Extracts the claim used for user ID in the active claim-based authentication scheme.
        ///
        /// for example, on JWT based claim systems, the ID is stored as an audience claim, but
        /// the same may not hold true for all authentication schemes. 
        /// </summary>
        /// <param name="principal">ClaimsPrincipal instance associated to the current authentication context.</param>
        /// <returns>Guid form of the principal's claimed ID.</returns>
        Guid ExtractFirstIdClaim(ClaimsPrincipal principal);
    }
}