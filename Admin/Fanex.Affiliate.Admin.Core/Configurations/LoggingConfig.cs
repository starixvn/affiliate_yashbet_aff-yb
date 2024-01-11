using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public static class LoggingConfig
    {
        public static void RegisterSentry()
        {
            var appSetting = DependencyResolver.Current.GetService<IAppSetting>();
            var env = appSetting.GetValue("Environment");
            var dsn = appSetting.GetValue("SentryDsn");
            var category = appSetting.GetValue("LoggingCategory");

            if (!string.IsNullOrEmpty(dsn))
            {
                LogManager.SetDefaultLogCategory(category)
                    .Use(new List<ILogEngine>
                    {
                        new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn(dsn),
                            Environment = env
                        })
                    });
            }
        }
    }
}
