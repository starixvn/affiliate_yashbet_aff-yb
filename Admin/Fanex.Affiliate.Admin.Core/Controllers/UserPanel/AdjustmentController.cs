using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using System;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class AdjustmentController : ReadOnlySessionBaseController
    {
        private readonly IAdjustmentService _adjustmentService;
        private const int DefaultBalanceBfValue = 0;

        public AdjustmentController(
            ISessionService sessionService,
            IAdjustmentService adjustmentService) : base(sessionService)
        {
            _adjustmentService = adjustmentService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetBalanceBroughtForward(int affId)
        {
            if (affId >= Validation.MinId)
            {
                return GenerateJsonNetResult(_adjustmentService.GetBalanceBroughtForward(affId));
            }

            return GenerateJsonNetResult(DefaultBalanceBfValue);

        }

        [FunctionAuthorize(FunctionName = Function.Payment, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult InsertAdjustment(AdjustmentParam param)
        {
            if (param.AffId >= Validation.MinId)
            {
                var absBalanceBf = Math.Abs(_adjustmentService.GetBalanceBroughtForward(param.AffId));

                if (ModelState.IsValid && param.IsValid(absBalanceBf))
                {
                    var adjustmentParam = new AdjustmentParam
                    {
                        AffId = param.AffId,
                        Amount = param.Amount,
                        Description = Server.HtmlEncode(param.Description)
                    };
                    AssignUserInfoParam(adjustmentParam);

                    return GenerateJsonNetResult(_adjustmentService.InsertAdjustment(adjustmentParam));
                }

                return GenerateErrResult(Lang.AmountIsInvalid);
            }

            return GenerateErrResult("Affiliate not exists!");
        }
    }
}
