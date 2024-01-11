using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Site.App_Start
{
    public static class LoggingConfig
    {
        public static void RegisterSentry()
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();
            var env = appSetting.GetValue("Environment");
            var backEndDsn = appSetting.GetValue("SentryBackEndDsn");
            var frontEndDsn = appSetting.GetValue("SentryFrontEndDsn");
            var category = appSetting.GetValue("LoggingCategory");
            var isRegisterSentry = string.Equals(appSetting.GetValue("IsRegisterSentry"), "true", System.StringComparison.CurrentCultureIgnoreCase);

            if (isRegisterSentry && !string.IsNullOrEmpty(backEndDsn) && !string.IsNullOrEmpty(frontEndDsn))
            {
                LogManager.SetDefaultLogCategory(category).Use(new List<ILogEngine>
                {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(backEndDsn),
                        FrontEndDsn = new Dsn(frontEndDsn),
                        Environment = env
                    })
                });
            }
        }
    }
}