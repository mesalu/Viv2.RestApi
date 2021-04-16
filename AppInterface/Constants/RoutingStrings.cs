namespace Viv2.API.AppInterface.Constants
{
    public class RoutingStrings
    {
        private const string BaseRouting = "/api/";
        public const string BaseController = BaseRouting + "[controller]";
        public const string AdminController = BaseRouting + "admin/[controller]";
    }
}
