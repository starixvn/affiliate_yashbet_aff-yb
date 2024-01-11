namespace Fanex.Affiliate.User.Core.Controllers
{
    using Extensions;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels.UserPanel.Statistics;

    [CustomAuthorize]
    public class TrackingStatisticsController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatisticsRepository _statisticsRepository;

        public TrackingStatisticsController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatisticsRepository statisticsRepository
            ) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _statisticsRepository = statisticsRepository;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            var affId = SessionService.UserSession.AffId;
            var isInternal = SessionService.IsInternal;

            var viewModel = new TrackingStatisticsViewModel
            {
                TrackingCodeList = _statisticsRepository.GetTrackingCode(affId, isInternal).Where(c => !c.MatchWith("[None]")),
                TrackingStatisticsList = _statisticsRepository.GetTrackingStatistics(affId, null, isInternal),
                IsDetail = false,
                CurrentLanguage = SessionService.CurrentLanguage
            };

            return View("~/Views/UserPanel/Statistics/TrackingStatistics/Index.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Index(string trackingCode)
        {
            var affId = SessionService.UserSession.AffId;
            var isInternal = SessionService.IsInternal;

            var viewModel = new TrackingStatisticsViewModel
            {
                TrackingStatisticsList = _statisticsRepository.GetTrackingStatistics(affId, trackingCode, isInternal),
                IsDetail = false,
                CurrentLanguage = SessionService.CurrentLanguage
            };

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_TrackingStatisticsContent.cshtml", viewModel);
        }


        [HttpPost]
        [AjaxOnly]
        public ActionResult TrackingStatisticsDetail(TrackingStatisticsDetailParam trackingStatisticsDetailParam)
        {
            trackingStatisticsDetailParam.AffId = SessionService.UserSession.AffId;
            trackingStatisticsDetailParam.IsInternal = SessionService.IsInternal;

            var viewModel = new TrackingStatisticsViewModel
            {
                TrackingStatisticsList = _statisticsRepository.GetTrackingStatisticsDetail(trackingStatisticsDetailParam),
                IsDetail = true
            };

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_TrackingStatisticsContent.cshtml", viewModel);
        }

    }
}