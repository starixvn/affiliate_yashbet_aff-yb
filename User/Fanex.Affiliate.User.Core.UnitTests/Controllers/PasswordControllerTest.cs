using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class PasswordControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly IAppSetting AppSetting;
        private readonly IAccountRepository AccountRepo;
        private readonly ICryptographyService CryptographyService;
        private readonly ILanguageService LanguageService;
        private readonly IEmailService EmailService;

        const string Email = "abc@gmail.com";
        const int InvalidUrlAddress = 1001;
        const int RecoverPasswordFailed = 1002;

        public PasswordControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            AppSetting = Substitute.For<IAppSetting>();
            AccountRepo = Substitute.For<IAccountRepository>();
            CryptographyService = Substitute.For<ICryptographyService>();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
            EmailService = Substitute.For<IEmailService>();
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrectView()
        {
            var controller = CreateController();

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Forgot_WhenCallWithEmptyEmail_ReturnNotOkResult(string email)
        {
            var controller = CreateController();

            var result = controller.Forgot(email) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.False(data.IsOk);
            Assert.Equal(Lang.ErrEmailRequire, data.Message);
        }

        [Fact]
        public void Forgot_WhenAccRepoReturnErrorCode_ReturnNotOkResult()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, controller.RouteData));
            AccountRepo.GetUserInfoByEmail(default(UserInfoByEmailParam))
                .ReturnsForAnyArgs(c =>
                {
                    (c[0] as UserInfoByEmailParam).ErrCode = 1001;
                    return new UserInfo();
                });
            SetupHttpContext();

            var result = controller.Forgot(Email) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.False(data.IsOk);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Forgot_WhenRepoReturnNoErrorCode_SendMail(bool isValid)
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, controller.RouteData));
            LanguageService.IsValid(Arg.Any<string>()).Returns(isValid);
            AccountRepo.GetUserInfoByEmail(default(UserInfoByEmailParam))
                .ReturnsForAnyArgs(c =>
                {
                    (c[0] as UserInfoByEmailParam).ErrCode = 0;
                    return new UserInfo();
                });
            SetupHttpContext();

            var result = controller.Forgot(Email) as JsonResult;
            var data = result.Data as ResultInfo;

            EmailService.ReceivedWithAnyArgs().SendRecoverPasswordEmail(default(RecoverPasswordEmailParam));
            Assert.True(data.IsOk);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData("", 1)]
        [InlineData("123", 0)]
        public void RecoverPassword_WhenCallWithEmptyId_RedirectToResultPageWithIsSuccessfulFalse(string vid, int uid)
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller, GetControlerRouteData());

            var result = controller.RecoverPassword(vid, uid) as RedirectToRouteResult;

            AssertRedirectToCorrectAction("RecoverPasswordResult", result);
            Assert.False((bool)result.RouteValues["isSuccessful"]);
        }

        [Fact]
        public void RecoverPassword_WhenRecoverPasswordFail_RedirectToResultPageWithErrorCode()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller, GetControlerRouteData());
            AccountRepo.RecoverPassword(default(PasswordRecoveryParam))
                .ReturnsForAnyArgs(c =>
                {
                    (c[0] as PasswordRecoveryParam).ErrCode = InvalidUrlAddress;
                    return new UserInfo();
                });

            var result = controller.RecoverPassword("abc", 1) as RedirectToRouteResult;

            AssertRedirectToCorrectAction("RecoverPasswordResult", result);
            Assert.False((bool)result.RouteValues["isSuccessful"]);
            Assert.Equal(1001, (int)result.RouteValues["errCode"]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RecoverPassword_WhenRecoverPasswordSuccess_RedirectToResultPageWithNoErrorCode(bool isValid)
        {
            LanguageService.IsValid(Arg.Any<string>()).Returns(isValid);
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller, GetControlerRouteData());
            AccountRepo.RecoverPassword(default(PasswordRecoveryParam))
                .ReturnsForAnyArgs(c =>
                {
                    (c[0] as PasswordRecoveryParam).ErrCode = 0;
                    return new UserInfo();
                });

            var result = controller.RecoverPassword("abc", 1) as RedirectToRouteResult;

            AssertRedirectToCorrectAction("RecoverPasswordResult", result);
            Assert.True((bool)result.RouteValues["isSuccessful"]);
            Assert.Equal(0, (int)result.RouteValues["errCode"]);
        }

        [Fact]
        public void RecoverPasswordResult_WhenCallWithErrorCodeRecoverPasswordFailed_ReturnMessageReconverPasswordFailed()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);

            var result = controller.RecoverPasswordResult(false, RecoverPasswordFailed) as ViewResult;

            Assert.NotNull(result);
            var data = result.Model as ResultInfo;
            Assert.False(data.IsOk);
        }

        [Fact]
        public void RecoverPasswordResult_WhenIsOk_ReturnIsSuccesfultResult()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);

            var result = controller.RecoverPasswordResult(true, 0) as ViewResult;

            Assert.NotNull(result);
            var data = result.Model as ResultInfo;
            Assert.True(data.IsOk);
            Assert.Contains(Lang.Yourlogindetail, data.Message);
        }

        [Fact]
        public void Update_WhenCallWithNoParam_ReturnView()
        {
            var controller = CreateController();
            AppSetting.RsaPublicKey.Returns("abc");

            var result = controller.Update() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal("abc", result.ViewBag.PublicKey);
        }

        [Fact]
        public void Update_WhenCallWithSessionIsInternal_ReturnErrorMessage()
        {
            var controller = CreateController();
            var param = new UpdatePasswordParam();
            SessionService.IsInternal.Returns(true);

            var result = controller.Update(param) as JsonResult;

            var data = result.Data as ResultInfo;
            Assert.NotNull(result);
            Assert.False(data.IsOk);
            Assert.Contains(Lang.ErrAuthorized, data.Message);
        }

        [Fact]
        public void Update_WhenModelNotValid_ReturnErrorMessage()
        {
            var controller = CreateController();
            var param = new UpdatePasswordParam();
            SessionService.IsInternal.Returns(false);
            controller.ModelState.AddModelError("abc", "need field abc");

            var result = controller.Update(param) as JsonResult;

            var data = result.Data as ResultInfo;
            Assert.NotNull(result);
            Assert.False(data.IsOk);
            Assert.Contains("need field abc", data.Message);
        }

        [Fact]
        public void Update_WhenNewPassWordDifferConfirmedPassword_ReturnErrorMessage()
        {
            var controller = CreateController();
            var param = new UpdatePasswordParam();
            SessionService.IsInternal.Returns(false);
            param.NewPassword = "123";
            param.ConfirmNewPassword = "1234";
            CryptographyService.EncryptPassword(Arg.Compat.Any<string>(), param.NewPassword).Returns("abcxyz");
            CryptographyService.EncryptPassword(Arg.Compat.Any<string>(), param.ConfirmNewPassword).Returns("123456");

            var result = controller.Update(param) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.NotNull(result);
            Assert.False(data.IsOk);
            Assert.Contains(Lang.ReenterNewPassw, data.Message);
        }

        [Fact]
        public void Update_WhenRepoUpdatePasswordFail_ReturnErrorMessange()
        {
            var controller = CreateController();
            var param = new UpdatePasswordParam();
            SessionService.IsInternal.Returns(false);
            SetupHttpContext();
            CryptographyService.EncryptPassword(default(string), default(string)).ReturnsForAnyArgs("12345");
            AccountRepo.UpdatePassword(default(UpdatePasswordParam)).ReturnsForAnyArgs(new ResultInfo { IsOk = false, Message = "error" });

            var result = controller.Update(param) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.NotNull(result);
            Assert.False(data.IsOk);
            Assert.Contains("error", data.Message);

        }

        [Fact]
        public void Update_whenRepoUpdateSuccess_ReturnOk()
        {
            var controller = CreateController();
            var param = new UpdatePasswordParam();
            SessionService.IsInternal.Returns(false);
            SetupHttpContext();
            CryptographyService.EncryptPassword(default(string), default(string)).ReturnsForAnyArgs("12345");
            AccountRepo.UpdatePassword(default(UpdatePasswordParam)).ReturnsForAnyArgs(new ResultInfo { IsOk = true });

            var result = controller.Update(param) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.NotNull(result);
            Assert.True(data.IsOk);
            Assert.Contains(Lang.Passwordchanged, data.Message);
        }

        private static void SetupHttpContext()
        {
            var request = new HttpRequest("abc", "http://abc.com", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
        }

        private static void AssertRedirectToCorrectAction(string actionName, RedirectToRouteResult result)
        {
            Assert.NotNull(result);
            Assert.Equal("Default", result.RouteName);
            Assert.Equal("Password", (string)result.RouteValues["controller"]);
            Assert.Equal(actionName, (string)result.RouteValues["action"]);
        }

        private static RouteData GetControlerRouteData()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Password");
            return routeData;
        }

        private PasswordController CreateController()
            => new PasswordController(SessionService, AppSetting, AccountRepo, CryptographyService, LanguageService, EmailService);
    }
}
