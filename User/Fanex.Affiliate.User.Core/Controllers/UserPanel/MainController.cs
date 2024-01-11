namespace Fanex.Affiliate.User.Core.Controllers
{
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using System.Web.Mvc;

    [CustomAuthorize]
    public class MainController : BaseController
    {
        private readonly IAppSetting _appSetting;

        public MainController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/UserPanel/Main/Index.cshtml");
        }
    }
}