using Fanex.Affiliate.Banner.Services.Impl;
using Fanex.Affiliate.Banner.Services.Interface;
using System.Web.Mvc;

namespace Fanex.Affiliate.Banner.Controllers
{
    public class UnderMaintenanceController : Controller
    {
        private readonly IUnderMaintenanceService _underMaintenanceService;

        public UnderMaintenanceController()
        {
            _underMaintenanceService = new UnderMaintenanceService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var maintenanceInfo = _underMaintenanceService.GetUnderMaintenanceTime();

            if (!maintenanceInfo.IsUnderMaintenanceTime)
            {
                return Redirect("~/Home");
            }

            return JavaScript($"document.getElementById('_affMediaText').innerHTML = 'System is currently unavailable due to scheduled maintenance from { maintenanceInfo.From} → { maintenanceInfo.To} (HK Time GMT +8). Thanks for your patience!';");
        }
    }
}