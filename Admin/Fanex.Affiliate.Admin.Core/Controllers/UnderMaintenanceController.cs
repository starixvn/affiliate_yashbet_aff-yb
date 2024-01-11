using Fanex.Affiliate.Admin.Core.Services.UnderMaintenanceService;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    public class UnderMaintenanceController : Controller
    {
        private readonly IUnderMaintenanceService _underMaintenanceService;

        public UnderMaintenanceController(IUnderMaintenanceService underMaintenanceService)
        {
            _underMaintenanceService = underMaintenanceService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (_underMaintenanceService.IsUnderMaintenance())
            {
                var underMaintenanceTime = _underMaintenanceService.GetUnderMaintenanceTime();

                return View(model: underMaintenanceTime);
            }

            return Redirect("~/Portal/Login");
        }
    }
}
