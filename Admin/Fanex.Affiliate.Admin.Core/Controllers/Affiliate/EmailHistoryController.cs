using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.EmailHistory;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Affiliate
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class EmailHistoryController : ReadOnlySessionBaseController
    {
        private readonly IEmailHistoryService _emailHistoryService;

        public EmailHistoryController(
            ISessionService sessionService,
            IEmailHistoryService emailHistoryService) : base(sessionService)
        {
            _emailHistoryService = emailHistoryService;
        }

        public ActionResult Index()
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AffiliateInfo, Url = Url.Content("~/Portal/Affiliate") },
                new SubMenu { Name = Lang.PaymentRequest, Url = Url.Content("~/Portal/PaymentRequest") },
                new SubMenu { Name = Lang.CommGroup, Url = Url.Content("~/Portal/RevenueGroup") },
                new SubMenu { Name = Lang.EmailHistory, Url = Url.Content("~/Portal/EmailHistory"), IsActive = true }
            };

            return View("~/Areas/Portal/Views/Affiliate/EmailHistory.cshtml", model: subMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetEmailHistoryList(EmailHistoryParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_emailHistoryService.GetEmailHistoryList(param));
            }

            return new JsonNetResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetEmailHistoryDetail(int selectedEmailId)
        {
            if (selectedEmailId >= Validation.MinId)
            {
                return GenerateJsonNetResult(_emailHistoryService.GetEmailHistoryDetail(selectedEmailId));
            }

            return new JsonNetResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Affiliate, IsHost = true)]
        public ActionResult DeleteEmailHistory(DeleteEmailHistoryParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_emailHistoryService.DeleteEmailHistory(param));
            }

            return GenerateModelStateErrResult();
        }
    }
}
