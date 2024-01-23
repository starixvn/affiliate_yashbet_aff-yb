namespace Fanex.Affiliate.User.Core.Controllers
{
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Filters;
    using System.Web.Mvc;

    [CustomAuthorize]
    public class MainController : BaseController
    {
        public MainController(
            ISessionService sessionService,
            ILanguageService languageService) : base(sessionService, languageService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/UserPanel/Main/Index.cshtml");
        }
    }
}