namespace Fanex.Affiliate.User.Core.Controllers
{
    using Constants;
    using DTO.UserPanel;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Filters;
    using MetalModels.UserPanel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Utilities;
    using ViewModels.UserPanel.Statistics;

    [CustomAuthorize]
    public class LandingPageStatisticsController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatisticsRepository StatisticsRepository;
        private readonly IMarketingService MarketingService;

        public LandingPageStatisticsController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatisticsRepository statisticsRepository,
            IMarketingService marketingService
            ) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            StatisticsRepository = statisticsRepository;
            MarketingService = marketingService;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            var userSession = SessionService.UserSession;
            var isInternal = SessionService.IsInternal;

            var now = DateTimeHelper.NowDateTimeOffset.ToOffset(new TimeSpan(GMT.PlusEight, 0, 0));
            var landingPageParam = new LandingPageStatisticsParam
            {
                AffId = userSession.AffId,
                IsInternal = isInternal,
                FromDate = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1),
                ToDate = now.DateTime
            };

            var viewModel = new LandingPageStatisticsViewModel
            {
                LanguageList = MarketingHelper.GetLanguageList(LanguageService.GetLanguageList(SessionService.IsInternal)),
                MemberSitePageList = MarketingService.GetLandingPageList(isInternal),
                FromDate = landingPageParam.FromDate,
                ToDate = landingPageParam.ToDate,
                DataList = GetLandingPageStatisticsData(landingPageParam),
                IsDetail = false
            };

            return View("~/Views/UserPanel/Statistics/LandingPageStatistics/Index.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Index(LandingPageStatisticsParam landingPageParam)
        {
            landingPageParam.AffId = SessionService.UserSession.AffId;
            landingPageParam.IsInternal = SessionService.IsInternal;

            var selectedLanguage = landingPageParam.LanguageCode;
            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                landingPageParam.LanguageCode = LanguageService.GetLanguageList()
                                                .FirstOrDefault(l => l.Code.Equals(selectedLanguage))
                                                ?.ShortedCode ?? SystemLanguage.DefaultShortedLanguageCode;
            }

            var viewModel = new LandingPageStatisticsViewModel
            {
                DataList = GetLandingPageStatisticsData(landingPageParam),
                IsDetail = false
            };

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_LandingPageStatisticsContent.cshtml", viewModel);
        }

        private IEnumerable<LandingPageStatistics> GetLandingPageStatisticsData(LandingPageStatisticsParam landingPageParam)
        {
            var dataList = StatisticsRepository.GetLandingPageStatistics(landingPageParam);

            foreach (var item in dataList)
            {
                var language = LanguageService.GetLanguageList().FirstOrDefault(p => p.ShortedCode == item.LanguageCode);

                item.LanguageName = language == null ? item.LanguageCode : language.NativeName;
            }

            return dataList;
        }
    }
}