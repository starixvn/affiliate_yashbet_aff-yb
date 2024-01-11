using Fanex.Caching;
using ONEbook.UM;
using ONEbook.UM.AspNetMVC.Filters;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.NETFramework;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public static class UMConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var umConfig = new UMClientConfiguration().Initialize();

            UMClientManager
               .UseConfig(umConfig)
               .UseCacheService(new UMCacheService(new CacheService()))
               .UseRestClient(new UMRestClient(umConfig))
               .UseHttpContext(new HttpContextHelper());

            var maintenanceService = new MaintenanceService(
                                  UMClientManager.RestClient,
                                  UMClientManager.CacheService,
                                  UMClientManager.UMClientConfigs,
                                  UMClientManager.HttpContext);

            filters.Add(new CheckUMFilterAttribute(maintenanceService, umConfig));
        }
    }
}
