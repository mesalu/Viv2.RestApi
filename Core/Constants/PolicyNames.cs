namespace Viv2.API.Core.Constants
{
    /// <summary>
    /// This class contains strings used to identify authorization policies, the strings contained correspond to
    /// specific claims required to satisfy the corresponding policy.
    /// </summary>
    public static  class PolicyNames
    {
        public const string UserAccess = "RequireUserAccess";
        public const string DaemonAccess = "RequireDaemonAccess";
    }
}