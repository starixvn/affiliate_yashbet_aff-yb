using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using ONEbook.UM.AspNetMVC.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Affiliate.User.Core.Controllers
{
    [CheckUMFilter]
    public class BaseController : Controller
    {
        protected readonly ILanguageService LanguageService;

        public BaseController(ISessionService sessionService,
            ILanguageService languageService)
        {
            SessionService = sessionService;
            LanguageService = languageService;
        }

        protected ISessionService SessionService { get; }

        protected virtual string RouteName => Constants.RouteName.DefaultRoute;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var favoriteLanguage = LanguageService.GetFavoriteLanguage();
            var sessionLang = SessionService.CurrentLanguage;
            if (favoriteLanguage != sessionLang)
            {
                requestContext.HttpContext.Response.Cookies.Add(new System.Web.HttpCookie("afffavoritelang" + AppSetting.Instance.SiteName, favoriteLanguage) { HttpOnly = true });
                SessionService.CurrentLanguage = favoriteLanguage;
            }
            LanguageService.SetCulture(favoriteLanguage);
        }

        protected RedirectToRouteResult RedirectToFixedAction(string actionName, object routeValue)
        {
            var routeDic = new RouteValueDictionary(routeValue)
            {
                { "controller", RouteData.Values["controller"] },
                { "action", actionName }
            };
            var result = RedirectToRoute(RouteName, routeDic);
            return result;
        }
    }
}
