namespace Fanex.Affiliate.User.Core.Controllers
{
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using System.Web.Mvc;
    using ViewModels.UserPanel.Statistics;

    [CustomAuthorize]
    public class StatisticsController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatisticsRepository statisticsRepository
            ) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _statisticsRepository = statisticsRepository;
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult StatisticsDetailBySite(StatisticsDetailBySiteParam statisticsDetailParam)
        {
            statisticsDetailParam.AffId = SessionService.UserSession.AffId;
            statisticsDetailParam.IsInternal = SessionService.IsInternal;

            var viewModel = new ReferrerStatisticsViewModel
            {
                DataList = _statisticsRepository.GetStatisticsDetailBySite(statisticsDetailParam),
                IsDetail = true,
                RowNumber = statisticsDetailParam.NRows
            };

            return PartialView("~/Views/UserPanel/Statistics/StatisticsPartial/_ReferrersStatisticsContent.cshtml", viewModel);
        }
    }
}