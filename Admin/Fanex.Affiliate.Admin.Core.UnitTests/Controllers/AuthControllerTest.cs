using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Login;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers
{
    public class AuthControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateAuthController();

            // Action
            var viewResult = controller.RecheckIn() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Auth/RecheckIn.cshtml", viewName);
        }

        [Fact]
        public void Ping_WhenCall_ReturnTrue()
        {
            // Arrange
            var controller = CreateAuthController();
            var expectedResult = new { IsKickedOut = true, Message = "Another session has logged in or your session is expired. Forced to logout!" };

            // Action
            var jsonResult = controller.Ping() as JsonResult;

            // Assert
            Assert.Equal(expectedResult.ToString(), jsonResult.Data.ToString());
        }

        [Fact]
        public void Ping_WhenCall_ReturnFalse()
        {
            // Arrange
            var sessionService = new Mock<ISessionService>();
            sessionService.Setup(s => s.OnUserId).Returns(123);
            sessionService.Setup(s => s.UserId).Returns(234);

            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.ReCheckin(It.IsAny<int>())).Returns(false);

            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Setup(m => m.IsAuthenticated).Returns(true);

            var mockUser = new Mock<IPrincipal>();
            mockUser.Setup(m => m.Identity.IsAuthenticated).Returns(true);

            var controller = new AuthController(sessionService.Object, authRepository.Object, Mock.Of<IAppSetting>())
            {
                ControllerContext = Mock.Of<ControllerContext>(ctx =>
                    ctx.HttpContext.Request == mockHttpRequest.Object &&
                    ctx.HttpContext.User == mockUser.Object
                )
            };
            var expectedResult = new { IsKickedOut = false, Message = string.Empty };

            // Action
            var jsonResult = controller.Ping() as JsonResult;

            // Assert
            Assert.Equal(expectedResult.ToString(), jsonResult.Data.ToString());
        }

        [Fact]
        public void Login_WhenCall_ReturnFalse()
        {
            // Arrange
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.Login(It.IsAny<LoginParam>())).Returns(new ResultInfo { Code = 999 });
            var controller = CreateAuthController(authRepository: authRepository.Object);

            // Action
            var jsonNetResult = controller.Login(new LoginParam()) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void Login_WhenCall_ReturnNotAccess()
        {
            // Arrange
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.Login(It.IsAny<LoginParam>())).Returns(new ResultInfo());
            authRepository.Setup(a => a.GetPermissionList(It.IsAny<int>())).Returns(new List<UserPermission>());
            var controller = CreateAuthController(authRepository: authRepository.Object);

            // Action
            var jsonNetResult = controller.Login(new LoginParam { UserId = 123 }) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Your account is not configured to access any functions! Please contact your administrator", resultInfo.Message);
        }

        private AuthController CreateAuthController(
            ISessionService sessionService = null,
            IAuthRepository authRepository = null,
            IAppSetting appSetting = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeAuthRepository = authRepository ?? Mock.Of<IAuthRepository>();
            var fakeAppSetting = appSetting ?? Mock.Of<IAppSetting>();

            var controller = new AuthController(fakeSessionService, fakeAuthRepository, fakeAppSetting);

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
