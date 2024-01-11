using ONEbook.UM.Models;

namespace Fanex.Affiliate.Admin.Core.Services.UnderMaintenanceService
{
    public interface IUnderMaintenanceService
    {
        UnderMaintenanceTime GetUnderMaintenanceTime();

        bool IsUnderMaintenance();
    }
}
