namespace Fanex.Affiliate.Link
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route configuration.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="routes">Route Collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults:
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
               name: "ChannelLink",
               url: "{pageId}/{affCode}/{lang}/{channel}/index.html",
               defaults:
               new
               {
                   controller = "Home",
                   action = "Link",
                   pageId = UrlParameter.Optional,
                   affCode = UrlParameter.Optional,
                   lang = UrlParameter.Optional,
                   channel = UrlParameter.Optional
               });

            routes.MapRoute(
               name: "Link",
               url: "{pageId}/{affCode}/{lang}/index.html",
               defaults:
               new
               {
                   controller = "Home",
                   action = "Link",
                   pageId = UrlParameter.Optional,
                   affCode = UrlParameter.Optional,
                   lang = UrlParameter.Optional
               });

            routes.IgnoreRoute("go");
        }
    }
}