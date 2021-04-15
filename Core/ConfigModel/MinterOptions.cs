namespace Viv2.API.Core.ConfigModel
{
    public class MinterOptions
    {
        /// <summary>
        /// Describes the issuer to be used by local minting.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Time in seconds a freshly minted token should last
        /// before timing out.
        /// </summary>
        public long TokenLifespan { get; set; }
        
        /// <summary>
        /// Like `TokenLifespan` but pertains to how long
        /// refresh tokens are valid for. 
        /// </summary>
        public long RefreshTokenLifespan { get; set; }
    }
}
