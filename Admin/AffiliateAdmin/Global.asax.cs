using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Data;
using Fanex.Data.SqlServer;
using System;
using System.Web;
using System.Web.Mvc;

namespace AffiliateAdmin
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            IoCConfig.Register();
            SslConfig.RegisterHttpSecurityProtocol(); //Fix abort https request

            UMConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            DbSettingProviderManager
                  .StartNewSession()
                  .WithSqlServer("~/App_Data/SpConfig/SpName.xml")
                  .Run();

            LoggingConfig.RegisterSentry();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError().GetBaseException();

            if (ex is HttpException)
            {
                return;
            }

            LogHelper.WriteError(ex);
        }
    }
}