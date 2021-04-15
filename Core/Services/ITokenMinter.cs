using System.Collections.Generic;
using System.Security.Claims;
using Viv2.API.Core.ConfigModel;

namespace Viv2.API.Core.Services
{
    public enum TokenType
    {
        UserAccess, DaemonAccess, Refresh
    }
    public interface ITokenMinter
    {
        /// <summary>
        /// Converts a set of claims - in the form of a claims identity - to an encoded token of some form.
        /// (Form depends on concrete implementation: e.g. JWS, JWE, PaSeTo, etc. are all options)
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string Mint(ClaimsIdentity identity, TokenType type);
        
        /// <summary>
        /// Decodes the given token, minted by the TokenMinter instance, and returns the claims that were embedded
        /// within the token. 
        /// </summary>
        /// <param name="minted"></param>
        /// <returns></returns>
        IEnumerable<Claim> ParseAccessToken(string minted);
        
        MinterOptions Options { get; }
    }
}
