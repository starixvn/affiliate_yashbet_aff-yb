using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.ChangeAdminPassword;
using Fanex.Affiliate.Admin.Core.Services.ChangeAdminPassword;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers
{
    public class ChangeAdminPasswordControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateChangeAdminPasswordController();

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void ChangePassword_WhenCallWithValidParam_ExcuteCorrect()
        {
            // Arrange
            var changeAdminPasswordService = new Mock<IChangeAdminPasswordService>();
            changeAdminPasswordService.Setup(u => u.ChangePassword(It.IsAny<ChangeAdminPasswordParam>())).Returns(new ResultInfo());
            var controller = CreateChangeAdminPasswordController(changeAdminPasswordService: changeAdminPasswordService.Object);

            // Action
            var result = controller.ChangePassword(new ChangeAdminPasswordParam { OldPassword = "123456789012345678901234567890", NewPassword = "234", ConfirmPassword = "234" }) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.True(data.IsSuccessful);
        }

        [Fact]
        public void ChangePassword_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateChangeAdminPasswordController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.ChangePassword(new ChangeAdminPasswordParam()) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.False(data.IsSuccessful);
        }

        private ChangeAdminPasswordController CreateChangeAdminPasswordController(
            ISessionService sessionService = null,
            IChangeAdminPasswordService changeAdminPasswordService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeChangeAdminPasswordService = changeAdminPasswordService ?? Mock.Of<IChangeAdminPasswordService>();

            var controller = new ChangeAdminPasswordController(fakeSessionService, fakeChangeAdminPasswordService);

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
