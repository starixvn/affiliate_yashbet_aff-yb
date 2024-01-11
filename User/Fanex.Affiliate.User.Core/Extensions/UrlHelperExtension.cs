using Fanex.Affiliate.User.Core.Utilities;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Extensions
{
    public static class UrlHelperExtension
    {
        private static readonly IAppSetting appSetting = DependencyResolver.Current.GetService<IAppSetting>();

        public static string RenderLink(this UrlHelper urlHelper, string bundleVirtualPath)
        {
            var link = appSetting.IsLocal ? urlHelper.Content(bundleVirtualPath) :
                                            urlHelper.Content(Regex.Replace(bundleVirtualPath, @"\.(js|css)$", ".min.$1"));

            return link + "?v=" + appSetting.JsVersion;
        }

        public static string VersionedLink(this UrlHelper urlHelper, string virtualPath)
        {
            return urlHelper.Content(virtualPath) + "?v=" + appSetting.JsVersion;
        }
    }
}
