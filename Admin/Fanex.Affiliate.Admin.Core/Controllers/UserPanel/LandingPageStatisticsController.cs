using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class LandingPageStatisticsController : ReadOnlySessionBaseController
    {
        private readonly ILandingPageStatisticsRepository _landingPageStatisticsRepository;

        public LandingPageStatisticsController(
            ISessionService sessionService,
            ILandingPageStatisticsRepository landingPageStatisticsRepository) : base(sessionService)
        {
            _landingPageStatisticsRepository = landingPageStatisticsRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetLandingPageStatisticsList(LandingPageStatisticsParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_landingPageStatisticsRepository.GetLandingPageStatisticsList(param));
            }

            return new JsonNetResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetLandingPageStatisticsDetailList(LandingPageStatisticsDetailParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_landingPageStatisticsRepository.GetLandingPageStatisticsDetailList(param));
            }

            return new JsonNetResult();
        }
    }
}
