using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.Member;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Member;
using Fanex.Affiliate.Admin.Core.ViewModels.Member;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Player
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class PlayerController : ReadOnlySessionBaseController
    {
        private readonly IMemberService _memberService;
        public PlayerController(
            ISessionService sessionService,
            IMemberService memberService) : base(sessionService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult Index(string affName)
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.Player, Url = Url.Content("~/Portal/Player"), IsActive = true }
            };

            var model = new MemberListViewModel
            {
                SubMenuList = subMenu,
                AffName = affName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetMemberList(MemberParam param)
        {
            return GenerateJsonNetResult(_memberService.GetMemberList(param));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetMemberDetail(int memberId)
        {
            return GenerateJsonNetResult(_memberService.GetMemberDetail(memberId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Affiliate, IsHost = true)]
        public ActionResult UpdateMember(UpdateMemberParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_memberService.UpdateMember(param));
            }

            return GenerateModelStateErrResult();
        }
    }
}
