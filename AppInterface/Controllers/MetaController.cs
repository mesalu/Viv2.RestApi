using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;

namespace Viv2.API.AppInterface.Controllers
{
    [Route(RoutingStrings.BaseController)]
    public class MetaController : ControllerBase 
    {
        [HttpGet("version")]
        public IActionResult GetVersionInfo()
        {
            return new OkObjectResult( new
            {
#if DEBUG
                Type = "Debug",
#else
                Type = "Release",
#endif
                Api = typeof(MetaController).Assembly.GetName().Version,
                
                // use a random class from Core for now.
                Core = typeof(Viv2.API.Core.Constants.ClaimNames).Assembly.GetName().Version
            });
        }
    }
}