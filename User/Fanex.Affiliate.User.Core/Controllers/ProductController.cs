namespace Fanex.Affiliate.User.Core.Controllers
{
    using CoreService;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.Utilities;
    using System.Web.Mvc;
    using ViewModels;

    public class ProductController : BaseLayoutController
    {
        public ProductController(IAppSetting appSetting, ISessionService sessionService, ILanguageService languageService)
            : base(appSetting, sessionService, languageService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new ProductPageViewModel
            {
                IsAuthenticated = HttpContext.Request.IsAuthenticated && SessionService.UserSession != null
            };
            AssignBaseLayoutViewModel(viewModel);

            return View("~/Views/Product/Index.cshtml", viewModel);
        }
    }
}