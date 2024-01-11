using Fanex.Affiliate.Link.Services.Impl;
using Fanex.Affiliate.Link.Services.Interface;
using System.Web.Mvc;

namespace Fanex.Affiliate.Link.Controllers
{
    public class UnderMaintenanceController : Controller
    {
        private const string MsgFormat = "<div style='text-align:center;'>System is currently unavailable due to scheduled maintenance." +
            "<br /> This downtime will last from <b>{0}</b> &rarr; <b>{1}</b> (HK Time GMT +8). Thanks for your patience!</div>";

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
                return RedirectToAction("PageNotFound", "Home");
            }

            ViewBag.Result = string.Format(MsgFormat, maintenanceInfo.From, maintenanceInfo.To);

            return View();
        }
    }
}