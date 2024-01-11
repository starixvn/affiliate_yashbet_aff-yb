using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class RevenueSharingStructureController : BaseLayoutController
    {
        public RevenueSharingStructureController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService) : base(appSetting, sessionService, languageService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new RevenueSharingStructureViewModel
            {
                DefaultCurrency = AppSetting.DefaultCurrency,
                MaxCommission = AppSetting.MaxCommission
            };
            AssignBaseLayoutViewModel(model);

            return View($"~/Views/CommissionStructure/Index.cshtml", model);
        }
    }
}