using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class TestingEmailControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly IAppSetting AppSetting;
        private readonly ILanguageService LanguageService;
        private readonly IMailProcessor MailProcessor;

        public TestingEmailControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            AppSetting = Substitute.For<IAppSetting>();
            MailProcessor = Substitute.For<IMailProcessor>();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
        }

        [Fact]
        public void Index_WhenCallWithIsInvalidUser_ReturnPageNotFound()
        {
            //Arrange
            SessionService.UserSession.Returns(null as UserSession);
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.Index() as HttpNotFoundResult;

            //Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrectView()
        {
            //Arrange
            AppSetting.IsLocal.Returns(true);
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.Empty(result.ViewName);
        }

        [Fact]
        public void Preview_WhenCallWithIsInvalidUser_ReturnPageNotFound()
        {
            //Arrange
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.Preview(new TestCriteria()) as HttpNotFoundResult;

            //Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData("ContactUs")]
        [InlineData("Csd")]
        [InlineData("RecoverPassword")]
        [InlineData("PaymentCancel")]
        [InlineData("PaymentRequest")]
        [InlineData("NewPassword")]
        [InlineData("Signup")]
        [InlineData("VerificationAccount")]
        [InlineData("abc")]
        public void Preview_WhenCall_ReturnCorrectView(string function)
        {
            //Arrange
            SessionService.IsInternal.Returns(true);
            MockingHelper.SetUpEnviroment();
            var controller = CreateTestingEmailController();
            controller = NSubtituteMockingHelper.FakeController(controller) as TestController;
            controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

            //Action
            var result = controller.Preview(new TestCriteria { Function = function }) as ViewResult;

            //Assert
            Assert.Empty(result.ViewName);
        }

        [Fact]
        public void Preview_WhenCallWithFormatTXT_ReturnCorrectView()
        {
            //Arrange
            SessionService.IsInternal.Returns(true);
            MockingHelper.SetUpEnviroment();
            var controller = CreateTestingEmailController();
            controller = NSubtituteMockingHelper.FakeController(controller) as TestController;
            controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

            //Action
            var result = controller.Preview(new TestCriteria { Format = "txt" }) as ViewResult;

            //Assert
            Assert.Empty(result.ViewName);
        }

        [Fact]
        public void Preview_WhenCallWithThrowException_ReturnCorrectView()
        {
            //Arrange
            SessionService.IsInternal.Returns(true);
            MockingHelper.SetUpEnviroment();
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.Preview(new TestCriteria()) as ViewResult;

            //Assert
            Assert.Empty(result.ViewName);
        }

        [Fact]
        public void SendEmail_WhenCallWithIsInvalidUser_ReturnPageNotFound()
        {
            //Arrange
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.SendEmail(new TestCriteria()) as HttpNotFoundResult;

            //Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData("ContactUs")]
        [InlineData("Csd")]
        [InlineData("RecoverPassword")]
        [InlineData("PaymentCancel")]
        [InlineData("PaymentRequest")]
        [InlineData("NewPassword")]
        [InlineData("Signup")]
        [InlineData("VerificationAccount")]
        [InlineData("abc")]
        public void SendEmail_WhenCall_SendMailSuccess(string function)
        {
            //Arrange
            AppSetting.GetAppMailIdByLanguageCode(Arg.Any<string>()).Returns("1");
            SessionService.IsInternal.Returns(true);
            MockingHelper.SetUpEnviroment();
            var controller = CreateTestingEmailController();
            controller = NSubtituteMockingHelper.FakeController(controller) as TestController;
            controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

            //Action
            var result = controller.SendEmail(new TestCriteria { Function = function, Language = Constant.DefaultLanguage, EmailAddress = "abc@gmail.com" }) as JsonResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void SendEmail_WhenCallWithThrowException_SendMailFailed()
        {
            //Arrange
            SessionService.IsInternal.Returns(true);
            SessionService.UserSession.Returns(new UserSession());
            MockingHelper.SetUpEnviroment();
            var controller = CreateTestingEmailController();

            //Action
            var result = controller.SendEmail(new TestCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result);
        }

        private TestController CreateTestingEmailController(ISessionService sessionService = null)
            => new TestController(sessionService ?? SessionService, LanguageService, AppSetting, MailProcessor);
    }
}
