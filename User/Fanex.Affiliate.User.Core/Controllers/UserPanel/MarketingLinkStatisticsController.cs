using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Extensions;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            var languages = MarketingHelper.GetLanguageList(LanguageService.GetLanguageList(SessionService.IsInternal));

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

            var model = GetMarketingPageStatisticsData(param)
                .GroupBy(s => s.ChannelName);

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_MarketingPageStatsticsContent.cshtml", model);
        }

        private IEnumerable<MarketingPageStatistics> GetMarketingPageStatisticsData(MarketingPageStatisticsParam marketingPageParam)
        {
            var dataList = _statisticsRepository.GetMarketingPageStatistics(marketingPageParam);
            var languageList = LanguageService.GetLanguageList();

            foreach (var item in dataList)
            {
                var language = languageList.FirstOrDefault(p => p.EnglishName.MatchWith(item.MarketName));

                item.Language = language == null ? item.Language : language.NativeName;
            }

            return dataList;
        }
    }
}