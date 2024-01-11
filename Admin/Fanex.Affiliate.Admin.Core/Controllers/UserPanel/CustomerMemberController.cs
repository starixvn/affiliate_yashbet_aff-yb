using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class CustomerMemberController : ReadOnlySessionBaseController
    {
        private readonly ISessionService _sessionService;
        private readonly ICustomerMemberService _customerMemberService;

        public CustomerMemberController(ISessionService sessionService, ICustomerMemberService customerMemberService) : base(sessionService)
        {
            _sessionService = sessionService;
            _customerMemberService = customerMemberService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetCustomerMember(CustomerMemberParam param)
        {
            if (ModelState.IsValid)
            {
                param.CanViewMemberEmail = _sessionService.UserPermissionList.Any(i => i.FunctionName.Equals(Function.MemberEmail, StringComparison.OrdinalIgnoreCase));

                return GenerateJsonNetResult(_customerMemberService.GetCustomerMember(param));
            }

            return new JsonNetResult();
        }
    }
}
