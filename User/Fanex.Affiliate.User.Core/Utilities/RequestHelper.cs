using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class RequestHelper
    {
        private const string IpRegex = "((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}";

        public static string GetClientIPAddress(HttpRequestBase httpRequest = null)
        {
            var request = httpRequest ?? new HttpRequestWrapper(HttpContext.Current.Request);
            var ips = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var ip = GetForwardedIPAddress(ips) ?? request.UserHostAddress;

            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        private static string GetForwardedIPAddress(string ips)
        {
            if (string.IsNullOrEmpty(ips))
            {
                return null;
            }

            var ipList = ips.Split(',').ToList();

            return ipList.Find(ip => Regex.IsMatch(ip, IpRegex));
        }
    }
}