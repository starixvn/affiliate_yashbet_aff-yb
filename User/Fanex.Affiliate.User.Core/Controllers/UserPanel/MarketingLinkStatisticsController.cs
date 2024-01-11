using System;
using System.Linq;
using System.Web.Mvc;
using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    [CustomAuthorize]
    public class MarketingLinkStatisticsController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatisticsRepository _statisticsRepository;

        public MarketingLinkStatisticsController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatisticsRepository statisticsRepository
            ) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _statisticsRepository = statisticsRepository;
        }

        public ActionResult Index()
        {
            var now = DateTimeHelper.NowDateTimeOffset.ToOffset(new TimeSpan(GMT.PlusEight, 0, 0));
            var startMonth = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1);
            var languages = LanguageService.GetLanguageList(SessionService.IsInternal).Select(l => new SelectListItem
            {
                Text = l.EnglishName,
                Value = l.Code
            });

            var viewModel = new MarketingPageStatisticsViewModel
            {
                FromDate = startMonth,
                ToDate = now.DateTime,
                IncludeInActiveLink = false
            };

            ViewBag.Languages = languages;

            return View("~/Views/UserPanel/Statistics/MarketingPageStatistics/Index.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Index(MarketingPageStatisticsViewModel vm)
        {
            var param = new MarketingPageStatisticsParam
            {
                AffId = SessionService.UserSession.AffId,
                FromDate = vm.FromDate,
                ToDate = vm.ToDate,
                IncludedInActive = vm.IncludeInActiveLink,
                LanguageCodes = vm.Language
            };

            var model = _statisticsRepository.GetMarketingPageStatistics(param)
                .GroupBy(s => s.ChannelName);

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_MarketingPageStatsticsContent.cshtml", model);
        }
    }
}