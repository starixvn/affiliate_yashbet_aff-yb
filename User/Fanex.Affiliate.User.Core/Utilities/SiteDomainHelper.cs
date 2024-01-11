namespace Fanex.Affiliate.User.Core.Utilities
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class SiteDomainHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3995:URI return values should not be strings", Justification = "<Pending>")]
        public static string GetRootUrl(Controller controller, IAppSetting appSetting = null)
        {
            var setting = appSetting ?? DependencyResolver.Current.GetService<IAppSetting>();
            var isUatOrProEnv = !setting.IsDev;
            var requestUrl = controller.Request.Url;

            return GetScheme(controller.Request, setting.IsProduction) + Uri.SchemeDelimiter + requestUrl.Host +
                (requestUrl.IsDefaultPort || isUatOrProEnv ? string.Empty : ":" + requestUrl.Port) + controller.Url.Content("~/");
        }

        private static string GetScheme(HttpRequestBase request, bool isProduction)
        {
            return isProduction ? request.Url.Scheme : GetSchemeByRequest(request);
        }

        private static string GetSchemeByRequest(HttpRequestBase request)
        {
            var isSecure = request.IsSecureConnection ||
                string.Equals(request.ServerVariables["HTTP_X_FORWARDED_PROTO"], Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);

            return isSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }
    }
}