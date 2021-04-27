using System.ComponentModel.DataAnnotations;

namespace Viv2.API.AppInterface.Dto
{
    /// <summary>
    /// Represents data necessary to be body-encoded for a refresh token exchange.
    /// Held separate from TokenExchangeRequest just in case of refactors / updates to either side.
    /// </summary>
    public class RefreshExchangeForm
    {
        //[Required]
        public string UserId { get; set; }
        
        //[Required]
        public string EncodedToken { get; set; }
    }
}