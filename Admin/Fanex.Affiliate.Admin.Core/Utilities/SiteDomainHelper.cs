using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public static class SiteDomainHelper
    {
        [SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public static string GetRootUrl(Controller controller)
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();
            var requestUrl = controller.Request.Url;
            var isUatOrProEnv = !appSetting.IsDev;

            return GetScheme(controller.Request) + Uri.SchemeDelimiter + requestUrl.Host +
                (requestUrl.IsDefaultPort || isUatOrProEnv ? string.Empty : ":" + requestUrl.Port) + controller.Url.Content("~/");
        }

        [SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public static string GetUserSiteUrl(int affType)
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();

            return affType == AffType.European ? appSetting.EuropeanAffSiteUrl : appSetting.AsianAffSiteUrl;
        }

        [SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public static string GetUserInternalSiteUrl(int affType)
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();

            return affType == AffType.European ? appSetting.InternalEuropeanAffiliateUrl : appSetting.InternalAsianAffiliateUrl;
        }

        private static string GetScheme(HttpRequestBase request)
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();

            if (appSetting != null && appSetting.IsProduction)
            {
                return request.Url.Scheme;
            }

            return IsSecure(request) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        private static bool IsSecure(HttpRequestBase request)
        {
            return request.IsSecureConnection || String.Equals(request.ServerVariables["HTTP_X_FORWARDED_PROTO"], "https", StringComparison.OrdinalIgnoreCase);
        }
    }
}