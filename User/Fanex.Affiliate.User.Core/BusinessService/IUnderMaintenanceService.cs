using ONEbook.UM.Models;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IUnderMaintenanceService
    {
        UnderMaintenanceTime GetUnderMaintenanceTime();

        bool IsUnderMaintenance();
    }
}
