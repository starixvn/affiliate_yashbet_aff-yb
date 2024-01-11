using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Moq;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class ContactUsControllerTest
    {
        [Fact]
        public void Index_WhenCallWithGet_ReturnCorrectView()
        {
            // Arrange
            var controller = CreateController();

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.ViewName;

            // Assert
            Assert.NotEmpty(result);
        }

        private static ContactUsController CreateController(
            ISessionService sessionService = null,
            IAppSetting appSetting = null,
            ILanguageService languageService = null)
        {
            sessionService = sessionService ?? MockingHelper.GetSessionService();
            appSetting = appSetting ?? Mock.Of<IAppSetting>();
            languageService = languageService ?? Mock.Of<ILanguageService>();

            var controller = new ContactUsController(appSetting, sessionService, languageService);
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
