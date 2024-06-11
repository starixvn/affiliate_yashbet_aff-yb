using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Data;
using Fanex.Data.SqlServer;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

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

        public override void Init()
        {
            base.Init();
            try
            {
                // Get the app name from config file...
                string appName = ConfigurationManager.AppSettings["ApplicationName"];
                if (!string.IsNullOrEmpty(appName))
                {
                    foreach (string moduleName in this.Modules)
                    {
                        IHttpModule module = this.Modules[moduleName];
                        if (module is SessionStateModule ssm)
                        {
                            FieldInfo storeInfo = typeof(SessionStateModule).GetField("_store", BindingFlags.Instance | BindingFlags.NonPublic);
                            SessionStateStoreProviderBase store = (SessionStateStoreProviderBase)storeInfo.GetValue(ssm);
                            if (store == null) //In IIS7 Integrated mode, module.Init() is called later
                            {
                                FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
                                HttpRuntime theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
                                FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
                                appNameInfo.SetValue(theRuntime, appName);
                            }
                            else
                            {
                                Type storeType = store.GetType();
                                if (storeType.Name.Equals("OutOfProcSessionStateStore"))
                                {
                                    FieldInfo uribaseInfo = storeType.GetField("s_uribase", BindingFlags.Static | BindingFlags.NonPublic);
                                    uribaseInfo.SetValue(storeType, appName);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }
    }
}