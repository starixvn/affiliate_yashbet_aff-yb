using System.Net;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public static class SslConfig
    {
        public static void RegisterHttpSecurityProtocol()
            => ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
    }
}
