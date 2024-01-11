namespace Fanex.Affiliate.Link.Services.Impl
{
    using Fanex.Affiliate.Link.Services.Interface;
    using ONEbook.UM;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;

    public class UnderMaintenanceService : IUnderMaintenanceService
    {
        private readonly IMaintenanceService _maintenanceService;
        public UnderMaintenanceService()
        {
            _maintenanceService = new MaintenanceService(
                                  UMClientManager.RestClient,
                                  UMClientManager.CacheService,
                                  UMClientManager.UMClientConfigs,
                                  UMClientManager.HttpContext);
        }

        public UnderMaintenanceTime GetUnderMaintenanceTime()
        {
            return _maintenanceService.GetUnderMaintenanceTime();
        }

        public bool IsUnderMaintenance()
        {
            return _maintenanceService.IsUnderMaintenance();
        }
    }
}