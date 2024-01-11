namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Enum;
    using Fanex.Affiliate.User.Core.Resources;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using ViewModels.UserPanel.Marketing;

    public class MediaGalleryController : BaseMarketingController
    {
        private IAccountService AccountService { get; }
        public MediaGalleryController(ISessionService sessionService, IAppSetting appSetting,
            ILanguageService languageService,
            IMarketingService marketingService,
            IAccountService accountService) :
            base(sessionService, appSetting, languageService, marketingService)
        {
            AccountService = accountService;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            var searchFormVM = GetSearchFormViewModel();
            ViewBag.SelectedLang = SessionService.UserSession.PreferredLanguage;

            return View("~/Views/UserPanel/AccountOverview/MediaGallery/Index.cshtml", searchFormVM);
        }

        private MediaGallerySearchFormViewModel GetSearchFormViewModel()
        {
            var isInternal = SessionService.IsInternal;
            var userSession = SessionService.UserSession;

            var channels = MarketingService.GetChannels(userSession.AffId);
            var categoryList = System.Enum.GetValues(typeof(AffiliateLinkType)).Cast<int>()
                .ToDictionary(t => t.ToString(), t => GetMediaMainCategory((AffiliateLinkType)t));
            var languageList = LanguageService.GetLanguageList(isInternal);
            var currencyList = MarketingService.GetCurrencyList(isInternal)
                .Select(i => new SelectListItem
                {
                    Value = i.Currency,
                    Text = Lang.ResourceManager.GetString(i.Currency.Trim()) ?? i.Currency
                });

            var sizeParam = new SizeParam
            {
                AffType = userSession.AffType,
                Language = userSession.PreferredLanguage
            };
            var sizeList = MarketingService.GetSizeList(sizeParam, isInternal)
                .Select(i => new SelectListItem { Value = i.Size, Text = i.Size });

            var refs = new List<RefLinkViewModel>
            {
                GetAffLinkTypeViewModel(),
            };

            var affId = SessionService.UserSession.AffId;
            if (!AccountService.GetGeneralAffInfo(affId).IsMaxSubLevel)
            {
                refs.Add(GetSubAffLinkTypeViewModel());
            }

            return new MediaGallerySearchFormViewModel
            {
                CategoryList = categoryList,
                Channels = channels,
                CurrencyList = currencyList,
                DeviceList = EnumHelper.GetSelectList(typeof(DeviceType)),
                Languages = languageList,
                RefLinks = refs,
                SizeList = sizeList
            };
        }

        private IEnumerable<SelectListItem> GetMediaMainCategory(AffiliateLinkType type)
            => MarketingService.GetCategoryList(true, type, SessionService.IsInternal)
            .Where(c => string.IsNullOrEmpty(c.SubCategory))
            .Select(c => new SelectListItem
            {
                Value = c.Category,
                Text = Lang.ResourceManager.GetString(Regex.Replace(c.Category, @"\s+", "")) ?? c.Category
            });
    }
}