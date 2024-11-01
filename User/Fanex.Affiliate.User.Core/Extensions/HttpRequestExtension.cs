using Fanex.Affiliate.User.Core.Constants;
using System;
using System.Web;

namespace Fanex.Affiliate.User.Core.Extensions
{
    public static class HttpRequestExtension
    {
        public static string GetClientIP(this HttpRequest request)
        {
            return GetClientIP(new HttpRequestWrapper(request));
        }

        public static string GetClientIP(this HttpRequestBase requestBase)
        {
            var xActualIP = requestBase.ServerVariables[HeaderKey.HTTP_X_ACTUAL_IP];
            var xForwardedFor = !string.IsNullOrWhiteSpace(xActualIP) ? xActualIP : requestBase.ServerVariables[HeaderKey.HTTP_X_FORWARDED_FOR];

            var clientIP = xForwardedFor?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]?.Trim();

            if (string.IsNullOrWhiteSpace(clientIP))
            {
                clientIP = requestBase.UserHostAddress;
            }

            return clientIP.Equals("::1") ? "127.0.0.1" : clientIP;
        }
    }
}
