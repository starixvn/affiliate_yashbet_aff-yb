using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class AuthControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly IAppSetting AppSetting;
        private readonly ILanguageService LanguageService;
        private readonly IAuthService AuthService;
        private readonly ICaptchaService CaptchaService;
        private readonly IAccountRepository AccountRepository;

        public AuthControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            AppSetting = Substitute.For<IAppSetting>();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
            AuthService = Substitute.For<IAuthService>();
            CaptchaService = Substitute.For<ICaptchaService>();
            AccountRepository = Substitute.For<IAccountRepository>();
        }

        [Fact]
        public void AuthenticationForm_WhenCallWithNoAuthenticationUser_SendPublicKey()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.HttpContext.User = Substitute.For<IPrincipal>();
            var identity = Substitute.For<IIdentity>();
            identity.IsAuthenticated.Returns(false);
            controller.HttpContext.User.Identity.Returns(identity);
            var key = "abcdxyz";
            AppSetting.RsaPublicKey.Returns(key);

            var result = controller.AuthenticationForm() as ViewResult;
            var model = result.Model as AuthenticationFormViewModel;

            Assert.Equal("~/Views/Auth/AuthenticationForm.cshtml", result.ViewName);
            Assert.Equal(key, model.PublicKey);
        }

        [Fact]
        public void AuthenticationForm_WhenCallWithNoSession_SendPublicKey()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.HttpContext.User = Substitute.For<IPrincipal>();
            var identity = Substitute.For<IIdentity>();
            identity.IsAuthenticated.Returns(true);
            controller.HttpContext.User.Identity.Returns(identity);
            SessionService.UserSession.ReturnsNull();
            var key = "abcdxyz";
            AppSetting.RsaPublicKey.Returns(key);

            var result = controller.AuthenticationForm() as ViewResult;
            var model = result.Model as AuthenticationFormViewModel;

            Assert.Equal("~/Views/Auth/AuthenticationForm.cshtml", result.ViewName);
            Assert.Equal(key, model.PublicKey);
        }

        [Fact]
        public void AuthenticationForm_WhenCallWithLoginUer_ReturnCoreectViewWithLoginUserName()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.HttpContext.User = Substitute.For<IPrincipal>();
            var identity = Substitute.For<IIdentity>();
            identity.IsAuthenticated.Returns(true);
            controller.HttpContext.User.Identity.Returns(identity);

            var result = controller.AuthenticationForm() as ViewResult;
            var model = result.Model as AuthenticationFormViewModel;

            Assert.Equal("~/Views/Auth/AuthenticationForm.cshtml", result.ViewName);
            Assert.Equal(SessionService.UserSession.UserName, model.UserName);
        }

        [Fact]
        public void SignIn_CallWhenInVerificationSession_ReturnResultFalse()
        {
            var controller = CreateController();
            SessionService.VerificationSession.Returns(new VerificationSession { AffId = 1 });

            var result = controller.SignIn(new UserLoginParam()) as JsonResult;
            var model = result.Data as LoginResultInfo;

            Assert.False(model.IsOk);
            Assert.True(model.ShouldVerifyAccount);
        }

        [Fact]
        public void SignIn_WhenCallNotAjaxRequest_ReturnResultNotOk()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);

            var result = controller.SignIn(new UserLoginParam()) as JsonResult;
            var model = result.Data as ResultInfo;

            Assert.False(model.IsOk);
            Assert.Equal(Lang.ErrAuthorized, model.Message);
        }

        [Fact]
        public void SignIn_WhenCallWithModelIsNotValid_ReturnResultNotOk()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            controller.ModelState.AddModelError("Error", "Need add field error");

            var result = controller.SignIn(new UserLoginParam()) as JsonResult;
            var model = result.Data as ResultInfo;

            Assert.False(model.IsOk);
            Assert.Contains("Need add field error", model.Message);
        }

        [Fact]
        public void SignIn_WhenCallWithNotValidCaptcha_ReturnResultNotOk()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(false);

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data as ResultInfo;

            Assert.False(model.IsOk);
            Assert.Equal(Lang.ValidationCode, model.Message);
        }

        [Fact]
        public void SignIn_WhenAccNeedVerify_ReturnShouldVerifyAccountResult()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(true);
            var loginResult = GetValidLoginResultViewModel();
            loginResult.Result.ShouldVerifyAccount = true;
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data;

            Assert.Equal(loginResult.Result, model);
        }

        [Fact]
        public void SignIn_WhenCallWithNotValidDomain_ReturnErrorResult()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(true);
            var loginResult = GetValidLoginResultViewModel();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string>());

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data as ResultInfo;

            Assert.False(model.IsOk);
            Assert.Equal(Lang.NotAllowOnDomain, model.Message);
        }

        [Fact]
        public void SignIn_WhenCallWithLoginResultIsNotOk_SessionNotSave()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(true);
            var loginResult = GetInvalidLoginResultViewModel();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asian" });

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data as ResultInfo;

            SessionService.DidNotReceive().AssignSessions(Arg.Compat.Any<AuthorizedUserInfo>());
            Assert.False(model.IsOk);
        }

        [Fact]
        public void SignIn_WhenCallWithUserInfoIsNull_SessionNotSave()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(true);
            var loginResult = GetLoginResultViewModelHasUserInfoIsNull();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asian" });

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data as ResultInfo;

            SessionService.DidNotReceive().AssignSessions(Arg.Compat.Any<AuthorizedUserInfo>());
            Assert.False(model.IsOk);
        }

        [Fact]
        public void SignIn_WhenCallWithLoginResultIsOk_SessionSave()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            var param = GetValidParam();
            CaptchaService.IsValid("", "", "").ReturnsForAnyArgs(true);
            var loginResult = GetValidLoginResultViewModel();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asian" });
            AccountRepository.GetAccountInfo(Arg.Any<int>()).Returns(new DTO.UserPanel.AccountInformation { DefaultBaseCurrency = 1 });

            var result = controller.SignIn(param) as JsonResult;
            var model = result.Data as ResultInfo;

            SessionService.Received().AssignSessions(Arg.Compat.Any<AuthorizedUserInfo>());
            Assert.True(model.IsOk);
        }

        [Fact]
        public void SignOut_WhenCall_RedirectToHome()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);

            var result = controller.SignOut() as RedirectResult;

            Assert.Contains("Home", result.Url);
        }

        [Fact]
        public void AdminLogin_ModelNotValid_RedirectToAccountOverView()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            controller.ModelState.AddModelError("abc", "Need field abc");

            var result = controller.AdminLogin(new InternalLoginParam()) as RedirectResult;

            Assert.Contains("/UserPanel/Main/#AccountOverView", result.Url);
        }

        [Fact]
        public void AdminLogin_WhenHashingKeyNotEqual_RedirectToPageNotFount()
        {
            var adminSessionId = "sessionId";
            var param = CreateInternalLoginParam();
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            AuthService.GetSessionId(default(int)).ReturnsForAnyArgs(adminSessionId);

            var result = controller.AdminLogin(param) as RedirectResult;

            Assert.Contains("PageNotFound", result.Url);
        }

        [Fact]
        public void AdminLogin_WhenAuthenFalse_RedirectToPageNotFound()
        {
            var adminSessionId = "sessionId";
            var chalengeStr = "abc";
            var hashed = Generator.GenerateSHA512String(adminSessionId + chalengeStr);
            var param = CreateInternalLoginParam();
            param.AccKey = hashed;
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            AuthService.GetSessionId(default(int)).ReturnsForAnyArgs(adminSessionId);
            var loginResult = GetValidLoginResultViewModel();
            loginResult.Result.IsOk = false;
            AuthService.GetLoginResult(default(UserLoginParam), true).ReturnsForAnyArgs(loginResult);

            var result = controller.AdminLogin(param) as RedirectResult;

            Assert.Contains("PageNotFound", result.Url);
        }

        [Fact]
        public void AdminLogin_WhenAuthenOk_UserSignIn()
        {
            var adminSessionId = "sessionId";
            var chalengeStr = "abc";
            var hashed = Generator.GenerateSHA512String(adminSessionId + chalengeStr);
            var param = CreateInternalLoginParam();
            param.AccKey = hashed;
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            AuthService.GetSessionId(default(int)).ReturnsForAnyArgs(adminSessionId);
            var loginResult = GetValidLoginResultViewModel();
            AuthService.GetLoginResult(default(UserLoginParam), true).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asian" });
            AccountRepository.GetAccountInfo(Arg.Any<int>()).Returns(new DTO.UserPanel.AccountInformation { DefaultBaseCurrency = 1 });

            var result = controller.AdminLogin(param) as RedirectResult;

            AuthService.ReceivedWithAnyArgs().SignIn(default(string), false);
            Assert.Contains("UserPanel/Main/#AccountOverView", result.Url);
        }

        [Fact]
        public void ReCheckIn_WhenAnotherOneLogin_SignOut()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            AuthService.Recheckin(1, false).ReturnsForAnyArgs(true);

            controller.ReCheckin();

            AuthService.Received().SignOut();
        }

        [Fact]
        public void ReCheckIn_WhenHasNotAnotherOneLogin_SignOut()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            AuthService.Recheckin(1, false).ReturnsForAnyArgs(false);

            controller.ReCheckin();

            AuthService.DidNotReceive().SignOut();
        }

        [Fact]
        public void CaptchaImage_WhenUserDefaultType_SessionSaveCaptchaCode()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            var captcha = "1234";
            CaptchaService.GenerateCaptcha().Returns(captcha);

            var result = controller.CaptchaImage();

            Assert.Equal(captcha, SessionService.CaptchaCode);
            Assert.Equal(typeof(FileContentResult), result.GetType());
        }

        [Fact]
        public void CaptchaImage_WhenTypeNotZero_SessionSaveSignUpCaptchaCode()
        {
            var controller = CreateController();
            NSubtituteMockingHelper.FakeController(controller);
            var captcha = "1234";
            CaptchaService.GenerateCaptcha().Returns(captcha);

            var result = controller.CaptchaImage(1);

            Assert.Equal(captcha, SessionService.SignUpCaptchaCode);
            Assert.Equal(typeof(FileContentResult), result.GetType());
        }

        public AuthController CreateController()
            => new AuthController(SessionService, AppSetting, LanguageService, AuthService, CaptchaService, AccountRepository);

        private UserLoginParam GetValidParam()
            => new UserLoginParam
            {
                IsVerifyingAccount = true,
                ValidationKey = "1234",
                LoginInfo = "1245",
                UserName = "abc"
            };

        private LoginResultViewModel GetValidLoginResultViewModel()
        {
            var userInfo = new AuthorizedUserInfo
            {
                AffId = 1,
                UserName = "test",
                AffType = "Asian"
            };
            var loginResult = new LoginResultInfo { ShouldVerifyAccount = false, IsOk = true };

            return new LoginResultViewModel
            {
                Result = loginResult,
                UserInfo = userInfo
            };
        }

        private LoginResultViewModel GetInvalidLoginResultViewModel()
        {
            var userInfo = new AuthorizedUserInfo
            {
                AffId = 1,
                UserName = "test",
                AffType = "Asian"
            };
            var loginResult = new LoginResultInfo { ShouldVerifyAccount = false, IsOk = false };

            return new LoginResultViewModel
            {
                Result = loginResult,
                UserInfo = userInfo
            };
        }

        private LoginResultViewModel GetLoginResultViewModelHasUserInfoIsNull()
        {
            var loginResult = new LoginResultInfo { ShouldVerifyAccount = false, IsOk = false };

            return new LoginResultViewModel
            {
                Result = loginResult
            };
        }

        private InternalLoginParam CreateInternalLoginParam()
            => new InternalLoginParam
            {
                AccId = 1,
                AccKey = "",
                ChalengeStr = "abc",
                AccOnId = 1,
                CustName = "cust"
            };
    }
}
