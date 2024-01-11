using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class VerificationAccountControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly IAppSetting AppSetting;
        private readonly ILanguageService LanguageService;
        private readonly ICaptchaService CaptchaService;
        private readonly IAccountRepository AccountRepo;
        private readonly IAuthService AuthService;
        private readonly IVerificationService VerificationService;

        public VerificationAccountControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            AppSetting = Substitute.For<IAppSetting>();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
            CaptchaService = Substitute.For<ICaptchaService>();
            AccountRepo = Substitute.For<IAccountRepository>();
            AuthService = Substitute.For<IAuthService>();
            VerificationService = Substitute.For<IVerificationService>();
        }

        [Fact]
        public void Index_WhenNotExistVerificationSession_RedirectToHome()
        {
            var controller = CreateController();
            SessionService.VerificationSession.ReturnsNull();

            var result = controller.Index() as RedirectResult;

            Assert.NotNull(result);
            Assert.Contains("Home", result.Url);
        }

        [Theory]
        [InlineData("abc@gmail.com", "••c@gmail.com")]
        [InlineData("x@gmail.com", "x@gmail.com")]
        [InlineData("xygmail.com", "")]
        public void Index_WhenInVerificationSession_ReturnVerficationView(string email, string expectedEmail)
        {
            var controller = CreateController();
            SessionService.VerificationSession.Returns(new VerificationSession());
            var userInfo = GetUserInfo();
            userInfo.Email = email;
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(userInfo);
            var hidedEmail = EmailHelper.HideEmailPart(userInfo.Email);

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as VerificationAccountViewModel;
            Assert.Equal(expectedEmail, model.Email);
            Assert.Equal(expectedEmail, hidedEmail);
        }

        [Fact]
        public void CaptchaImage_WhenCall_ReturnImageCaptchaPng()
        {
            var controller = CreateController();
            var sessionSevice = new VerificationSession();
            SessionService.VerificationSession.Returns(sessionSevice);
            var imageByte = new byte[] { 0x20, 0x20, 0x20 };
            CaptchaService.GenerateImageBuffer(null, 0, 0, false).ReturnsForAnyArgs(imageByte);

            var result = controller.CaptchaImage() as FileContentResult;

            CaptchaService.ReceivedWithAnyArgs().GenerateImageBuffer(null, 0, 0, false);
            Assert.NotNull(result);
            Assert.Equal("image/png", result.ContentType);
            Assert.Equal(imageByte, result.FileContents);
        }

        [Fact]
        public void SendCode_WhenEmailNull_ReturnErrorMessage()
        {
            var controller = CreateController();

            var result = controller.SendCode(null) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as ResultInfo;
            Assert.False(data.IsOk);
            Assert.Equal(Error.General, data.Message);
        }

        [Fact]
        public void SendCode_WhereVerifactionFailed_SessionUpdate()
        {
            var controller = CreateController();
            var verficationSession = new VerificationSession();
            SessionService.VerificationSession.Returns(verficationSession);
            var sendCodeResult = GetFailedSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();

            var result = controller.SendCode("abc@gmail.com") as JsonResult;

            Assert.NotNull(result);
            Assert.Equal(sendCodeResult.NewCounter, verficationSession.Counter);
            Assert.Equal(sendCodeResult.NewSendCodeTime, verficationSession.SentCodeTime);
        }

        [Fact]
        public void SendCode_WhereVerifactionSucess_SessionUpdate()
        {
            var controller = CreateController();
            var verficationSession = new VerificationSession();
            SessionService.VerificationSession.Returns(verficationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();

            var result = controller.SendCode("abc@gmail.com") as JsonResult;

            Assert.NotNull(result);
            Assert.Equal(sendCodeResult.NewCounter, verficationSession.Counter);
            Assert.Equal(sendCodeResult.NewSendCodeTime, verficationSession.SentCodeTime);
        }

        [Fact]
        public void VerifyAccount_WhenVerificatoinNotExist_ReturnErrorMessage()
        {
            var controller = CreateController();
            SessionService.VerificationSession.ReturnsNull();
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
            Assert.Contains(Error.General, data.Message);
        }

        [Fact]
        public void VerifyAccount_WhenVerifyFailed_SignIn()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession { AffId = 1 };
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(new UserInfo());
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(true);
            VerificationService.IsValidOtp(null).ReturnsForAnyArgs(true);
            var loginResult = GetFailedLoginResult();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asia" });

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
        }

        [Fact]
        public void VerifyAccount_WhenValidateLoginRequestFail_ReturnError()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession();
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(false);

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
            Assert.Contains(Lang.ValidationCode, data.Message);
        }

        [Fact]
        public void VerifyAccount_WhenReciveNotVaildOtp_ReturnError()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession();
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(new UserInfo());
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(true);
            VerificationService.IsValidOtp(null).ReturnsForAnyArgs(false);

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
            Assert.Contains(Error.VerificationFailed, data.Message);
        }

        [Fact]
        public void VerifyAccount_WhenUserInfoNotGetable_ReturnErrorMessage()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession();
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(new UserInfo());
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(true);
            VerificationService.IsValidOtp(null).ReturnsForAnyArgs(true);
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(new LoginResultViewModel());

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
            Assert.Contains(Error.LoginWrongInfo, data.Message);
        }

        [Fact]
        public void VerifyAccount_WhenNotValidDomain_ReturnErrorMessage()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession { AffId = 1 };
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(new UserInfo());
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(true);
            VerificationService.IsValidOtp(null).ReturnsForAnyArgs(true);
            var loginResult = GetSuccessLoginResult();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.False(data.IsOk);
            Assert.Contains(Lang.NotAllowOnDomain, data.Message);
        }

        [Fact]
        public void VerifyAccount_WhenVerifySuccess_SignIn()
        {
            var controller = CreateController();
            var verificationSession = new VerificationSession { AffId = 1 };
            SessionService.VerificationSession.Returns(verificationSession);
            var sendCodeResult = GetSuccessSendCodeResult();
            VerificationService.SendCode(null).ReturnsForAnyArgs(sendCodeResult);
            SetUpAppSetting();
            AccountRepo.GetUserInfoById(0, false).ReturnsForAnyArgs(new UserInfo());
            AccountRepo.GetAccountInfo(0).ReturnsForAnyArgs(CreateAccountInfo());
            CaptchaService.IsValid(null, null, null).ReturnsForAnyArgs(true);
            VerificationService.IsValidOtp(null).ReturnsForAnyArgs(true);
            var loginResult = GetSuccessLoginResult();
            AuthService.GetLoginResult(null, false).ReturnsForAnyArgs(loginResult);
            AppSetting.AllowedAffiliateTypes.Returns(new List<string> { "Asia" });

            var result = controller.VerifyAccount(new MetalModels.VerificationAccountParam()) as JsonResult;

            AuthService.ReceivedWithAnyArgs().SignIn(null, false);
            Assert.NotNull(result);
            var data = result.Data as VerifyAccountResult;
            Assert.True(data.IsOk);
        }

        private static AccountInformation CreateAccountInfo()
            => new AccountInformation
            {
                Address = "",
                AffCurrency = "",
                AffId = 1,
                City = "aa",
                PlayerUsername = "username"
            };

        private static UserInfo GetUserInfo()
            => new UserInfo
            {
                Email = "abc@yahoo.com",
                AffId = 1,
                FirstName = "First",
                LastName = "Last",
                UserName = "abc"
            };

        private static LoginResultViewModel GetFailedLoginResult()
            => new LoginResultViewModel
            {
                Result = new LoginResultInfo
                {
                    IsOk = false
                },
                UserInfo = new DTO.Auth.AuthorizedUserInfo
                {
                    AffId = 1,
                    AffType = "Asia"
                },
                VerificationSession = new VerificationSession()
            };

        private static LoginResultViewModel GetSuccessLoginResult()
            => new LoginResultViewModel
            {
                Result = new LoginResultInfo
                {
                    IsOk = true
                },
                UserInfo = new DTO.Auth.AuthorizedUserInfo
                {
                    AffId = 1,
                    AffType = "Asia"
                },
                VerificationSession = new VerificationSession()
            };

        private void SetUpAppSetting()
        {
            AppSetting.GetValue("ExpirationCodeInMinute").Returns("20");
            AppSetting.GetValue("SendCodeIntervalInMinute").Returns("20");
        }

        private SendCodeResult GetSuccessSendCodeResult()
            => new SendCodeResult
            {
                CanSendCode = true,
                IsValidEmail = true,
                NewCounter = 1,
                NewSendCodeTime = 1234
            };

        private SendCodeResult GetFailedSendCodeResult()
            => new SendCodeResult
            {
                CanSendCode = false,
                IsValidEmail = false
            };
        private VerificationAccountController CreateController()
        {
            var controller = new VerificationAccountController(SessionService,
                AppSetting,
                LanguageService,
                CaptchaService,
                AccountRepo,
                AuthService,
                VerificationService);

            NSubtituteMockingHelper.FakeController(controller);
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);
            return controller;
        }
    }
}
