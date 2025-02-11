namespace Fanex.Affiliate.Link
{
    using Fanex.Affiliate.Link.App_Start;
    using Fanex.Affiliate.Link.Helpers;
    using Fanex.Data;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Routing;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Global for Affiliate link.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            LoggingConfig.RegisterSentry();
            DbSettingProviderManager
                        .StartNewSession()
                        .Register(true)
                        .Run();

            UMConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Error arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is HttpException)
            {
                return;
            }

            LogHelper.WriteError(exception, $"Request url: {Request.Url}");
        }
    }
}