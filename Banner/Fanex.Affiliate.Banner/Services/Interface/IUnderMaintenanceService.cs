using ONEbook.UM.Models;

namespace Fanex.Affiliate.Banner.Services.Interface
{
    public interface IUnderMaintenanceService
    {
        UnderMaintenanceTime GetUnderMaintenanceTime();

        bool IsUnderMaintenance();
    }
}
