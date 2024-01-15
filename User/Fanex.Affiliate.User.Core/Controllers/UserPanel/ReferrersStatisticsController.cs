namespace Fanex.Affiliate.User.Core.Controllers
{
    using Enum;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.Constants;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using System;
    using System.Web.Mvc;
    using ViewModels.UserPanel.Statistics;

    [CustomAuthorize]
    public class ReferrersStatisticsController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatisticsRepository _statisticsRepository;

        public ReferrersStatisticsController(
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
            var now = DateTimeHelper.NowDateTimeOffset.ToOffset(new TimeSpan(GMT.PlusEight, 0, 0));
            var referrerStatisticsParam = new ReferrerStatisticsParam
            {
                AffId = SessionService.UserSession.AffId,
                IsInternal = SessionService.IsInternal,
                FromDate = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1),
                ToDate = now.DateTime,
                DeviceType = (byte)DeviceType.PCDevice
            };

            var viewModel = new ReferrerStatisticsViewModel
            {
                DataList = _statisticsRepository.GetReferrerStatistics(referrerStatisticsParam),
                FromDate = referrerStatisticsParam.FromDate,
                ToDate = referrerStatisticsParam.ToDate
            };

            return View("~/Views/UserPanel/Statistics/ReferrersStatistics/Index.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Index(ReferrerStatisticsParam referrerStatisticsParam)
        {
            referrerStatisticsParam.AffId = SessionService.UserSession.AffId;
            referrerStatisticsParam.IsInternal = SessionService.IsInternal;

            var viewModel = new ReferrerStatisticsViewModel
            {
                DataList = _statisticsRepository.GetReferrerStatistics(referrerStatisticsParam)
            };

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_ReferrersStatisticsContent.cshtml", viewModel);
        }
    }
}