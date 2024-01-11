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
    public class AdministratorControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateAdministratorController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/Administrator");

            controller.Url = urlHelperMock.Object;

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void GetUserList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(t => t.GetUserList(0, "", "")).Returns(new List<AdministratorInfo> { new AdministratorInfo() });
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.GetUserList("", "") as JsonNetResult;
            var data = result.Data as List<AdministratorInfo>;

            // Assert
            Assert.Single(data);
        }

        [Fact]
        public void GetUserDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var selectedUserId = 123;
            int passedParam = 0;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(t => t.GetUserDetail(selectedUserId)).Returns(new AdministratorInfo { UserId = 123 }).Callback<int>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.GetUserDetail(selectedUserId) as JsonNetResult;
            var data = result.Data as AdministratorInfo;

            // Assert
            Assert.Equal(selectedUserId, data.UserId);
            Assert.Equal(selectedUserId, passedParam);
        }

        [Fact]
        public void InsertUser_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUserParam { UserName = "abc", Password = "123" };
            InsertUserParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.InsertUser(It.IsAny<InsertUserParam>())).Returns(new ResultInfo()).Callback<InsertUserParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.InsertUser(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal("Encodedabc", passedParam.UserName);
        }

        [Fact]
        public void InsertUser_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertUser(new InsertUserParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("", resultInfo.Message);
        }

        [Fact]
        public void InsertExistedUser_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertExistedUserParam { UserName = "abc" };
            InsertExistedUserParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.InsertExistedUser(It.IsAny<InsertExistedUserParam>())).Returns(new ResultInfo()).Callback<InsertExistedUserParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.InsertExistedUser(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal("Encodedabc", passedParam.UserName);
        }

        [Fact]
        public void InsertExistedUser_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertExistedUser(new InsertExistedUserParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("", resultInfo.Message);
        }

        [Fact]
        public void UpdateUser_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new UpdateUserParam { AccountId = 123 };
            UpdateUserParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.UpdateUser(It.IsAny<UpdateUserParam>())).Returns(new ResultInfo()).Callback<UpdateUserParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.UpdateUser(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.AccountId, passedParam.AccountId);
        }

        [Fact]
        public void UpdateUser_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateUser(new UpdateUserParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("", resultInfo.Message);
        }

        [Fact]
        public void ChangeStatus_WhenCallWithValidParam_DeleteSuccessful()
        {
            // Arrange
            var param = new ChangeStatusParam { SelectedUserId = 123 };
            ChangeStatusParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.ChangeStatus(It.IsAny<ChangeStatusParam>())).Returns(new ResultInfo()).Callback<ChangeStatusParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.ChangeStatus(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void ChangeStatus_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.ChangeStatus(new ChangeStatusParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Account Id is invalid", resultInfo.Message);
        }

        [Fact]
        public void GetAccountPolicyList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var groupId = 123;
            int passedParam = 0;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(t => t.GetAccountPolicyList(groupId)).Returns(new List<PolicyInfo> { new PolicyInfo { FunctionId = 123 } }).Callback<int>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.GetAccountPolicyList(groupId) as JsonNetResult;
            var data = result.Data as List<PolicyInfo>;

            // Assert
            Assert.Single(data);
            Assert.Equal(groupId, passedParam);
        }

        [Fact]
        public void GetPolicyListByGroupIds_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var groupIds = "123";
            var passedParam = string.Empty;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(t => t.GetPolicyListByGroupIds(groupIds)).Returns(new List<PolicyInfo> { new PolicyInfo { FunctionId = 123 } }).Callback<string>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.GetPolicyListByGroupIds(groupIds) as JsonNetResult;
            var data = result.Data as List<PolicyInfo>;

            // Assert
            Assert.Single(data);
            Assert.Equal(groupIds, passedParam);
        }

        [Fact]
        public void UpdateAccountPolicy_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new UpdatePolicyParam { GroupPolicies = "100000034,123,0;" };
            UpdatePolicyParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.UpdateAccountPolicy(It.IsAny<UpdatePolicyParam>())).Returns(new ResultInfo()).Callback<UpdatePolicyParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.UpdateAccountPolicy(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal("Encoded100000034,123,0;", passedParam.GroupPolicies);
        }

        [Fact]
        public void UpdateAccountPolicy_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateAccountPolicy(new UpdatePolicyParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("The account policy is invalid.", resultInfo.Message);
        }

        [Fact]
        public void ChangePassword_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new UpdatePasswordParam { AccountId = 123, Password = "abc", ConfirmPassword = "abc" };
            UpdatePasswordParam passedParam = null;
            var administratorService = new Mock<IAdministratorService>();
            administratorService.Setup(s => s.ChangePassword(It.IsAny<UpdatePasswordParam>())).Returns(new ResultInfo()).Callback<UpdatePasswordParam>(p => passedParam = p);
            var controller = CreateAdministratorController(administratorService: administratorService.Object);

            // Action
            var result = controller.ChangePassword(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.AccountId, passedParam.AccountId);
        }

        [Fact]
        public void ChangePassword_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdministratorController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.ChangePassword(new UpdatePasswordParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("", resultInfo.Message);
        }

        private AdministratorController CreateAdministratorController(
        ISessionService sessionService = null,
        IAdministratorService administratorService = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeAdministratorService = administratorService ?? Mock.Of<IAdministratorService>();

            var controller = new AdministratorController(fakeSessionService, fakeAdministratorService);

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
