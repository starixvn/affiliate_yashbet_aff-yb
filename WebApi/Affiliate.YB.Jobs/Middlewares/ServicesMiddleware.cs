using Affiliate.YB.Helpers;
using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SharpRaven;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Affiliate.YB.Jobs.Middlewares
{
    public static class ServicesMiddleware
    {
        private static AppSettings appSettings;

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            ServicesMiddleware.appSettings = appSettings;
            services.AddSingleton<IDynamicRepository, DynamicRepository>();

            InitApi(services);
            InitLogging(services);
            InitFanexData(configuration);
        }

        public static void InitLogging(IServiceCollection services)
        {
            LogManager
                    .SetDefaultLogCategory("Affiliate.YB.DataApi")
                    .Use(new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(appSettings.SentryDsn)
                    }));

            services.AddSingleton(Logger.Log);
        }

        public static void InitFanexData(IConfiguration configuration)
        {
            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: appSettings.SpPath)
                .Run();
        }

        private static void InitApi(IServiceCollection services)
        {
            Func<HttpClient> CreateHttpClient = () =>
            {
                var httpCLient = HttpClientFactory.Create(
                      new AuthenticatedHttpClientHandler(appSettings.AuthenticationKey)
                      {
                          Proxy = appSettings.ProxySetting != null && appSettings.ProxySetting.IsUseProxy
                              ? new WebProxy(new Uri(appSettings.ProxySetting.ProxyUrl))
                              : null,
                          AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                      });
                httpCLient.BaseAddress = new Uri(appSettings.LicenseeDataAPI);
                httpCLient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                return httpCLient;
            };

            services.AddSingleton(RestService.For<ILicenseeApi>(CreateHttpClient()));
            services.AddSingleton(RestService.For<IPromotionApi>(CreateHttpClient()));
            services.AddSingleton(RestService.For<IProduct9WApi>(CreateHttpClient()));
        }
    }
}