using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class TrackingStatisticsController : ReadOnlySessionBaseController
    {
        private readonly ITrackingStatisticsService _trackingStatisticsService;

        public TrackingStatisticsController(
            ISessionService sessionService,
            ITrackingStatisticsService trackingStatisticsService) : base(sessionService)
        {
            _trackingStatisticsService = trackingStatisticsService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTrackingStatisticsList(TrackingStatisticsParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_trackingStatisticsService.GetTrackingStatisticsList(param));
            }

            return new JsonNetResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTrackingStatisticsDetailList(TrackingStatisticsDetailParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_trackingStatisticsService.GetTrackingStatisticsDetailList(param));
            }

            return new JsonNetResult();
        }
    }
}
