using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fanex.Affiliate.Admin.Core.Extensions
{
    public static class HttpRequestExtension
    {
        private const string IPv4Regex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        public static string GetClientIP(this HttpRequest request)
        {
            return GetClientIP(new HttpRequestWrapper(request));
        }

        public static string GetClientIP(this HttpRequestBase requestBase)
        {
            var xActualIP = requestBase.ServerVariables[HeaderKey.HTTP_X_ACTUAL_IP];
            var xForwardedFor = !string.IsNullOrWhiteSpace(xActualIP) ? xActualIP : requestBase.ServerVariables[HeaderKey.HTTP_X_FORWARDED_FOR];

            var clientValidIP = GetValidIPAddress(xForwardedFor);

            if (string.IsNullOrWhiteSpace(clientValidIP))
            {
                clientValidIP = requestBase.UserHostAddress;
            }

            return clientValidIP.Equals("::1") ? "127.0.0.1" : clientValidIP;
        }

        private static string GetValidIPAddress(string ips)
        {
            if (string.IsNullOrEmpty(ips))
            {
                return null;
            }

            var ipList = ips.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim())
                        .Where(i => Regex.IsMatch(i, IPv4Regex, RegexOptions.None, TimeSpan.FromSeconds(2)));

            return ipList.FirstOrDefault();
        }
    }
}
