using ONEbook.UM;
using ONEbook.UM.Models;
using ONEbook.UM.Services;

namespace Fanex.Affiliate.Admin.Core.Services.UnderMaintenanceService
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
            var underMaintenanceTime = _maintenaceService.GetUnderMaintenanceTime();
            underMaintenanceTime.From = GetTimeUM(underMaintenanceTime.From);
            underMaintenanceTime.To = GetTimeUM(underMaintenanceTime.To);

            return underMaintenanceTime;
        }

        public bool IsUnderMaintenance()
        {
            return _maintenaceService.IsUnderMaintenance();
        }

        private static string GetTimeUM(string dateTime)
        {
            const int TWO = 2;
            var result = dateTime.Split(' ');

            if (result.Length >= TWO)
            {
                return result[1];
            }

            return result[0];
        }
    }
}
