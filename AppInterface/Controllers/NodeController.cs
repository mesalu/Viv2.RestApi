using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.Core.Constants;

namespace Viv2.API.AppInterface.Controllers
{
    /// <summary>
    /// An interface for accessing and manipulating NodeController data.
    /// This class name is reduced to just `node`, as NodeControllerController was... unideal.
    /// </summary>
    [Authorize(Policy = PolicyNames.UserAccess)]
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class NodeController : ControllerBase
    {
        
    }
}