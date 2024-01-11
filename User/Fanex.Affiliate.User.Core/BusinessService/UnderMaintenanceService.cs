using ONEbook.UM;
using ONEbook.UM.Models;
using ONEbook.UM.Services;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class UnderMaintenanceService : IUnderMaintenanceService
    {
        private readonly IMaintenanceService _maintenaceService;
        public UnderMaintenanceService()
        {
            _maintenaceService = new MaintenanceService(
                                      UMClientManager.RestClient,
                                      UMClientManager.CacheService,
                                      UMClientManager.UMClientConfigs,
                                      UMClientManager.HttpContext);
        }

        public UnderMaintenanceTime GetUnderMaintenanceTime()
        {
            return _maintenaceService.GetUnderMaintenanceTime();
        }

        public bool IsUnderMaintenance()
        {
            return _maintenaceService.IsUnderMaintenance();
        }
    }
}
