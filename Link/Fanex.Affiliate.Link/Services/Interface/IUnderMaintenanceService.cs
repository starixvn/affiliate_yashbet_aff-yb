namespace Fanex.Affiliate.Link.Services.Interface
{
    using ONEbook.UM.Models;

    interface IUnderMaintenanceService
    {
        UnderMaintenanceTime GetUnderMaintenanceTime();

        bool IsUnderMaintenance();
    }
}
