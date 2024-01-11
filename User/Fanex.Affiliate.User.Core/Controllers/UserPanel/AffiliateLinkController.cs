namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using ONEbook.UM.AspNetMVC.Filters;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.UserPanel.Marketing;

    [CheckUMFilter]
    [CustomAuthorize]
    public class AffiliateLinkController : BaseMarketingController
    {
        private IAccountService AccountService { get; }

        public AffiliateLinkController(ISessionService sessionService,
            IAppSetting appSetting,
            ILanguageService languageService,
            IMarketingService marketingService,
            IAccountService accountService)
            : base(sessionService,
                appSetting,
                languageService,
                marketingService)
        {
            AccountService = accountService;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            var affId = SessionService.UserSession.AffId;
            var langs = LanguageService.GetLanguageList(SessionService.IsInternal);
            var channels = MarketingService.GetChannels(affId);

            var refs = new List<RefLinkViewModel>
            {
                GetAffLinkTypeViewModel(),
            };

            if (!AccountService.GetGeneralAffInfo(affId).IsMaxSubLevel)
            {
                refs.Add(GetSubAffLinkTypeViewModel());
            }

            var vm = new AffiliateLinkViewModel
            {
                Channels = channels,
                Languages = langs,
                RefLinks = refs
            };

            ViewBag.SelectedLang = SessionService.UserSession.PreferredLanguage;

            return View("~/Views/UserPanel/AccountOverview/AffiliateLink/Index.cshtml", vm);
        }
    }
}