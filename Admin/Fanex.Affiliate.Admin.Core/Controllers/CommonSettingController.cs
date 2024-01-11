using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Repository.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class CommonSettingController : BaseController
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly ITrackingStatisticsService _trackingStatisticsService;

        public CommonSettingController(
            ILandingPageRepository landingPageRepository,
            ITrackingStatisticsService trackingStatisticsService
            )
        {
            _landingPageRepository = landingPageRepository;
            _trackingStatisticsService = trackingStatisticsService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetPageList()
        {
            return GenerateJsonNetResult(_landingPageRepository.GetLandingPageList());
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTrackingCodeList(int affId)
        {
            return GenerateJsonNetResult(_trackingStatisticsService.GetTrackingCodeList(affId));
        }
    }
}