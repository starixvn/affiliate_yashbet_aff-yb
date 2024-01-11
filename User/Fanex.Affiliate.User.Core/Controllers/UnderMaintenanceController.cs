using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class UnderMaintenanceController : Controller
    {
        private readonly IAppSetting _appSetting;
        private readonly ISessionService _sessionService;
        protected readonly ILanguageService _languageService;
        private readonly IUnderMaintenanceService _underMaintenanceService;

        public UnderMaintenanceController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService, IUnderMaintenanceService underMaintenanceService)
        {
            _appSetting = appSetting;
            _sessionService = sessionService;
            _languageService = languageService;
            _underMaintenanceService = underMaintenanceService;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var favoriteLanguage = _languageService.GetFavoriteLanguage();
            var sessionLang = _sessionService.CurrentLanguage;
            if (favoriteLanguage != sessionLang)
            {
                _sessionService.CurrentLanguage = favoriteLanguage;
            }
            _languageService.SetCulture(favoriteLanguage);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var maintenanceInfo = _underMaintenanceService.GetUnderMaintenanceTime();

            if (!maintenanceInfo.IsUnderMaintenanceTime)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            ViewBag.UmTimeFrom = maintenanceInfo.From;
            ViewBag.UmTimeTo = maintenanceInfo.To;

            return View("~/Views/UnderMaintenance/Index.cshtml");
        }
    }
}
