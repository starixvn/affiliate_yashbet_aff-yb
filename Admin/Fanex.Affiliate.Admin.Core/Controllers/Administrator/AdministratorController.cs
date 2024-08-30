using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Administrator
{
    [FunctionAuthorize(FunctionName = Function.UserControl)]
    public class AdministratorController : ReadOnlySessionBaseController
    {
        private readonly IAdministratorService _administratorService;

        public AdministratorController(
            ISessionService sessionService,
            IAdministratorService administratorService) : base(sessionService)
        {
            _administratorService = administratorService;
        }

        public ActionResult Index()
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AdminList, Url = Url.Content("~/Portal/Administrator"), IsActive = true },
                new SubMenu { Name = Lang.DeptList, Url = Url.Content("~/Portal/Department") }
            };

            return View(subMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetAccountPolicyList(int groupId)
        {
            return GenerateJsonNetResult(_administratorService.GetAccountPolicyList(groupId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetPolicyListByGroupIds(string groupIds)
        {
            return GenerateJsonNetResult(_administratorService.GetPolicyListByGroupIds(groupIds));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetUserList(string userName, string firstLetter, int groupId = 0)
        {
            return GenerateJsonNetResult(_administratorService.GetUserList(groupId, userName, firstLetter));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetUserDetail(int selectedUserId)
        {
            return GenerateJsonNetResult(_administratorService.GetUserDetail(selectedUserId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult InsertUser(InsertUserParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = new InsertUserParam
                {
                    UserName = Server.HtmlEncode(param.UserName.Trim()),
                    Password = CryptographyHelper.Encrypt(param.Password.Trim()),
                    FirstName = Server.HtmlEncode(param.FirstName),
                    LastName = Server.HtmlEncode(param.LastName),
                    GroupId = param.GroupId
                };

                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_administratorService.InsertUser(encodeParam));
            }

            return GenerateModelStateErrResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult InsertExistedUser(InsertExistedUserParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = new InsertExistedUserParam
                {
                    UserName = Server.HtmlEncode(param.UserName.Trim()),
                    GroupId = param.GroupId
                };

                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_administratorService.InsertExistedUser(encodeParam));
            }

            return GenerateModelStateErrResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult UpdateUser(UpdateUserParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = new UpdateUserParam
                {
                    AccountId = param.AccountId,
                    FirstName = Server.HtmlEncode(param.FirstName),
                    LastName = Server.HtmlEncode(param.LastName),
                    GroupId = param.GroupId
                };

                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_administratorService.UpdateUser(encodeParam));
            }

            return GenerateModelStateErrResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult ChangeStatus(ChangeStatusParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_administratorService.ChangeStatus(param));
            }

            return GenerateErrResult("Account Id is invalid");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult UpdateAccountPolicy(UpdatePolicyParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = new UpdatePolicyParam
                {
                    GroupPolicies = Server.HtmlEncode(param.GroupPolicies.Trim())
                };
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_administratorService.UpdateAccountPolicy(encodeParam));
            }

            return GenerateErrResult("The account policy is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult ChangePassword(UpdatePasswordParam param)
        {
            if (ModelState.IsValid)
            {
                param.Password = CryptographyHelper.Encrypt(param.Password);
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_administratorService.ChangePassword(param));
            }

            return GenerateModelStateErrResult();
        }
    }
}
