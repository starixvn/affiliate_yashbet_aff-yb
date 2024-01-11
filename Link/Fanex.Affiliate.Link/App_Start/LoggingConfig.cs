using System;
using System.Collections.Generic;
using System.Configuration;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;

namespace Fanex.Affiliate.Link
{
    public static class LoggingConfig
    {
        public static void RegisterSentry()
        {
            var appSetting = ConfigurationManager.AppSettings;
            var env = appSetting.Get("Environment");
            var backEndDsn = appSetting.Get("SentryBackEndDsn");
            var category = appSetting.Get("LoggingCategory");
            var isRegisterSentry = string.Equals(appSetting.Get("IsRegisterSentry"), "true", StringComparison.CurrentCultureIgnoreCase);

            if (isRegisterSentry && !string.IsNullOrEmpty(backEndDsn))
            {
                LogManager.SetDefaultLogCategory(category).Use(new List<ILogEngine>
                {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(backEndDsn),
                        Environment = env
                    })
                });
            }
        }
    }
}