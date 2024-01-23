using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppSetting _appSetting;
        public HomeController(
            ISessionService sessionService,
            IAppSetting appSetting,
            ILanguageService languageService) : base(sessionService, languageService)

        {
            _appSetting = appSetting;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var isAuthenticated = Request.IsAuthenticated && SessionService.UserSession != null;

            var viewModel = new HomeViewModel
            {
                IsAuthenticated = isAuthenticated,
                Language = SessionService.CurrentLanguage,
                ShortedLanguageCode = SystemLanguage.GetUppercaseShortedCode(SessionService.CurrentLanguage),
                SignUpLink = isAuthenticated ? Url.Content("~/UserPanel/Main/#AccountOverview") : Url.Content("~/SignUp"),
                BannerLink = isAuthenticated ? Url.Content("~/UserPanel/Main/#AccountOverview#MediaGallery") : "javascript:;",
                PromotionYBLink = GetPromotionLink()
            };

            return View($"~/Views/Home/Index.cshtml", viewModel);
        }

        private string GetPromotionLink()
        {
            var segments = Request.Url.Host.Split('.');
            var domainName = segments.Length > 1 ? segments[segments.Length - 2] : string.Empty;

            return string.Format(_appSetting.PromotionYBLink, domainName).Replace("..", ".");
        }
    }
}