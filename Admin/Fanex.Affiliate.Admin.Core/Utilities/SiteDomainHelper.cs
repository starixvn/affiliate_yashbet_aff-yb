using Fanex.Affiliate.Admin.Core.Configurations;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public static class SiteDomainHelper
    {
        private static readonly IAppSetting appSetting = DependencyResolver.Current.GetService<IAppSetting>();

        public static string GetRootUrl(Controller controller)
        {
            var requestUrl = controller.Request.Url;
            var isUatOrProEnv = !appSetting.IsDev;

            return GetScheme(controller.Request) + Uri.SchemeDelimiter + requestUrl.Host +
                (requestUrl.IsDefaultPort || isUatOrProEnv ? string.Empty : ":" + requestUrl.Port) + controller.Url.Content("~/");
        }

        private static string GetScheme(HttpRequestBase request)
        {
            if (appSetting != null && appSetting.IsProduction)
            {
                return request.Url.Scheme;
            }

            return IsSecure(request) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        private static bool IsSecure(HttpRequestBase request)
        {
            return request.IsSecureConnection || string.Equals(request.ServerVariables["HTTP_X_FORWARDED_PROTO"], "https", StringComparison.OrdinalIgnoreCase);
        }
    }
}