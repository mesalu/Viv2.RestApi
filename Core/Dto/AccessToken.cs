namespace Viv2.API.Core.Dto
{
    /// <summary>
    /// Representation of an access token as delivered by Core.
    ///
    /// Important distinction: This DTO model represents how Core expects the token to be passed around and represented
    /// it does not (directly) represent an actual access token as minted by the active TokenMinter. (That's the `Token`
    /// field).
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// Encoded token value
        /// </summary>
        public string Token { get; init; }
        
        /// <summary>
        /// In how many seconds does the token expire.
        /// </summary>
        public long ExpiresIn { get; init; }
    }
}
