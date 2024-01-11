using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Administrator
{
    [FunctionAuthorize(FunctionName = Function.UserControl)]
    public class DepartmentController : ReadOnlySessionBaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(
            ISessionService sessionService,
            IDepartmentService departmentService) : base(sessionService)
        {
            _departmentService = departmentService;
        }

        public ActionResult Index()
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AdminList, Url = Url.Content("~/Portal/Administrator") },
                new SubMenu { Name = Lang.DeptList, Url = Url.Content("~/Portal/Department"), IsActive = true }
            };

            return View("~/Areas/Portal/Views/Administrator/Department.cshtml", subMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetDepartmentList()
        {
            return GenerateJsonNetResult(_departmentService.GetDepartmentList());
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetDepartmentDetail(int selectedDepartmentId)
        {
            return GenerateJsonNetResult(_departmentService.GetDepartmentDetail(selectedDepartmentId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult InsertDepartment(InsertUpdateDepartmentParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_departmentService.InsertDepartment(encodeParam));
            }

            return GenerateErrResult("Please input Department's name.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult UpdateDepartment(InsertUpdateDepartmentParam param)
        {
            var isValidDepartmentId = param.DepartmentId >= Validation.MinId;
            if (ModelState.IsValid && isValidDepartmentId)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_departmentService.UpdateDepartment(encodeParam));
            }

            return GenerateErrResult(isValidDepartmentId ? "Please input Department's name." : "Department Id is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult DeleteDepartment(DeleteDepartmentParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_departmentService.DeleteDepartment(param));
            }

            return GenerateErrResult("Department id is incorrect.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetRolePolicy(int departmentId)
        {
            return GenerateJsonNetResult(_departmentService.GetRolePolicy(departmentId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult UpdateRolePolicy(UpdatePolicyParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = new UpdatePolicyParam
                {
                    GroupPolicies = Server.HtmlEncode(param.GroupPolicies.Trim())
                };
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_departmentService.UpdateRolePolicy(encodeParam));
            }

            return GenerateErrResult("The role policy is invalid.");
        }

        private InsertUpdateDepartmentParam EncodeParam(InsertUpdateDepartmentParam param)
        {
            return new InsertUpdateDepartmentParam
            {
                DepartmentId = param.DepartmentId,
                DepartmentName = Server.HtmlEncode(param.DepartmentName),
                Description = Server.HtmlEncode(param.Description)
            };
        }
    }
}
