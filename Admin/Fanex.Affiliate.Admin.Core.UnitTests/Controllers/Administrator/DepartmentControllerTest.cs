using Fanex.Affiliate.Admin.Core.Controllers.Administrator;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Administrator;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Administrator
{
    public class DepartmentControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateDepartmentController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/Department");

            controller.Url = urlHelperMock.Object;

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Administrator/Department.cshtml", viewName);
        }

        [Fact]
        public void GetDepartmentList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(t => t.GetDepartmentList()).Returns(new List<DepartmentInfo> { new DepartmentInfo() });
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.GetDepartmentList() as JsonNetResult;
            var data = result.Data as List<DepartmentInfo>;

            // Assert
            Assert.Single(data);
        }

        [Fact]
        public void GetDepartmentDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var selectedDepartmentId = 123;
            int passedParam = 0;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(t => t.GetDepartmentDetail(selectedDepartmentId)).Returns(new DepartmentInfo { DepartmentId = 123 }).Callback<int>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.GetDepartmentDetail(selectedDepartmentId) as JsonNetResult;
            var data = result.Data as DepartmentInfo;

            // Assert
            Assert.Equal(selectedDepartmentId, data.DepartmentId);
            Assert.Equal(selectedDepartmentId, passedParam);
        }

        [Fact]
        public void InsertDepartment_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDepartmentParam { DepartmentName = "abc" };
            InsertUpdateDepartmentParam passedParam = null;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(s => s.InsertDepartment(It.IsAny<InsertUpdateDepartmentParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDepartmentParam>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.InsertDepartment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.DepartmentId, passedParam.DepartmentId);
        }

        [Fact]
        public void InsertDepartment_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateDepartmentController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertDepartment(new InsertUpdateDepartmentParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input Department's name.", resultInfo.Message);
        }

        [Fact]
        public void UpdateDepartment_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDepartmentParam { DepartmentId = 123, DepartmentName = "abc" };
            InsertUpdateDepartmentParam passedParam = null;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(s => s.UpdateDepartment(It.IsAny<InsertUpdateDepartmentParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDepartmentParam>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.UpdateDepartment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.DepartmentId, passedParam.DepartmentId);
        }

        [Fact]
        public void UpdateDepartment_WhenCallWithInvalidDepartmentId_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDepartmentParam { DepartmentName = "abc" };
            var controller = CreateDepartmentController();

            // Action
            var result = controller.UpdateDepartment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Department Id is invalid.", resultInfo.Message);
        }

        [Fact]
        public void UpdateDepartment_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDepartmentParam { DepartmentId = 123, DepartmentName = "abc" };
            var controller = CreateDepartmentController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateDepartment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input Department's name.", resultInfo.Message);
        }

        [Fact]
        public void DeleteDepartment_WhenCallWithValidParam_DeleteSuccessful()
        {
            // Arrange
            var param = new DeleteDepartmentParam { DepartmentId = 123 };
            DeleteDepartmentParam passedParam = null;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(s => s.DeleteDepartment(It.IsAny<DeleteDepartmentParam>())).Returns(new ResultInfo()).Callback<DeleteDepartmentParam>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.DeleteDepartment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteDepartment_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateDepartmentController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteDepartment(new DeleteDepartmentParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Department id is incorrect.", resultInfo.Message);
        }

        [Fact]
        public void GetRolePolicy_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var selectedDepartmentId = 123;
            int passedParam = 0;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(t => t.GetRolePolicy(selectedDepartmentId)).Returns(new List<PolicyInfo> { new PolicyInfo { FunctionId = 123 } }).Callback<int>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.GetRolePolicy(selectedDepartmentId) as JsonNetResult;
            var data = result.Data as List<PolicyInfo>;

            // Assert
            Assert.Single(data);
            Assert.Equal(selectedDepartmentId, passedParam);
        }

        [Fact]
        public void UpdateRolePolicy_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new UpdatePolicyParam { GroupPolicies = "100000034,123,0;" };
            UpdatePolicyParam passedParam = null;
            var departmentService = new Mock<IDepartmentService>();
            departmentService.Setup(s => s.UpdateRolePolicy(It.IsAny<UpdatePolicyParam>())).Returns(new ResultInfo()).Callback<UpdatePolicyParam>(p => passedParam = p);
            var controller = CreateDepartmentController(departmentService: departmentService.Object);

            // Action
            var result = controller.UpdateRolePolicy(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal("Encoded100000034,123,0;", passedParam.GroupPolicies);
        }

        [Fact]
        public void UpdateRolePolicy_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateDepartmentController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateRolePolicy(new UpdatePolicyParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("The role policy is invalid.", resultInfo.Message);
        }

        private DepartmentController CreateDepartmentController(
        ISessionService sessionService = null,
        IDepartmentService departmentService = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeDepartmentService = departmentService ?? Mock.Of<IDepartmentService>();

            var controller = new DepartmentController(fakeSessionService, fakeDepartmentService);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            return controller;
        }
    }
}
