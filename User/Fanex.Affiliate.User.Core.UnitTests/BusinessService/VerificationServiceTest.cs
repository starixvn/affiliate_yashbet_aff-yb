using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System;
using System.Security.Cryptography;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class VerificationServiceTest
    {
        private readonly ITwoFactorAuthService TwoFactorAuthService;
        private readonly IAccountRepository AccountRepo;
        private readonly IEmailService MailService;
        private readonly ICryptographyService CryptoService;
        private readonly IWrapperUtility WrapperUtility;
        private readonly IVerificationRepository VerificationRepo;

        public VerificationServiceTest()
        {
            TwoFactorAuthService = Substitute.For<ITwoFactorAuthService>();
            AccountRepo = Substitute.For<IAccountRepository>();
            MailService = Substitute.For<IEmailService>();
            CryptoService = Substitute.For<ICryptographyService>();
            WrapperUtility = Substitute.For<IWrapperUtility>();
            VerificationRepo = Substitute.For<IVerificationRepository>();
        }

        [Fact]
        public void SendCode_WhenInNotTimeToSendCode_ReturnFailSendCodeResult()
        {
            var service = CreateService();
            var param = CreateSendCodeParam();
            var now = new DateTime(2020, 1, 1);
            var interval = 10;
            var lastSendTime = now.AddMinutes(-5);
            param.SentCodeTime = lastSendTime.Ticks;
            param.IntervalInMinute = interval;
            WrapperUtility.GetNowDateTime().Returns(now);

            var result = service.SendCode(param);

            Assert.False(result.CanSendCode);
        }

        [Fact]
        public void SendCode_WhenParamNotHaveSameEmailAsUser_ReturnInvalidEmail()
        {
            var service = CreateService();
            var param = CreateSendCodeParam();
            var userInfo = CreateUserInfo();
            userInfo.Email = "anxcj@com.vn";
            WrapperUtility.GetNowDateTime().Returns(DateTime.Now);
            AccountRepo.GetUserInfoById(1, false).ReturnsForAnyArgs(userInfo);

            var result = service.SendCode(param);

            Assert.False(result.IsValidEmail);
        }

        [Fact]
        public void SendCode_WhenUserNotHaveSercretKey_GenSecretKeyForUser()
        {
            var service = CreateService();
            var param = CreateSendCodeParam();
            var userInfo = CreateUserInfo();
            userInfo.SecretKey = null;
            WrapperUtility.GetNowDateTime().Returns(DateTime.Now);
            WrapperUtility.GetRandomGenerator().Returns(new Random());
            AccountRepo.GetUserInfoById(1, false).ReturnsForAnyArgs(userInfo);
            TwoFactorAuthService.GenerateHotp(null, 0).ReturnsForAnyArgs("12345");
            AccountRepo.CreateSecretKey(0, null).ReturnsForAnyArgs(101, 102);

            service.SendCode(param);

            AccountRepo.ReceivedWithAnyArgs(2).CreateSecretKey(0, null);
            Assert.False(string.IsNullOrWhiteSpace(userInfo.SecretKey));
        }

        [Fact]
        public void SendCode_WhenUserHaveSecretKey_NotGenSecretKey()
        {
            var service = CreateService();
            var param = CreateSendCodeParam();
            var userInfo = CreateUserInfo();
            WrapperUtility.GetNowDateTime().Returns(DateTime.Now);
            WrapperUtility.GetRandomGenerator().Returns(new Random());
            AccountRepo.GetUserInfoById(1, false).ReturnsForAnyArgs(userInfo);
            TwoFactorAuthService.GenerateHotp(null, 0).ReturnsForAnyArgs("12345");

            service.SendCode(param);

            AccountRepo.DidNotReceiveWithAnyArgs().CreateSecretKey(0, null);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsValidOtp_WhenLoginFail_UpdateVerificationInfo(bool isOK)
        {
            MockingHelper.SetUpLogEngineEnviroment();
            var service = CreateService();
            var param = CreateVerificationOtpParam();
            var verificationInfo = new VerificationInfo
            {
                DeviceCount = 1,
                FailedLoginCount = 1
            };

            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsForAnyArgs(verificationInfo);
            VerificationRepo.UpdateVerificationInfo(null).ReturnsForAnyArgs(new ResultInfo { IsOk = isOK });

            service.IsValidOtp(param);

            VerificationRepo.ReceivedWithAnyArgs().UpdateVerificationInfo(null);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsValidOtp_WhenLoginOkAndUseValidOtp_InsertUserDeviceToRepo(bool isOK)
        {
            var service = CreateService();
            var param = CreateVerificationOtpParam();
            var authMsg = param.LoginInfo + param.HashKey + param.Captcha + param.OtpCode;
            param.HashMessage = CryptographyHelper.SHA256Hash(authMsg, param.HashKey);
            var verificationInfo = new VerificationInfo
            {
                DeviceCount = 1,
                FailedLoginCount = null
            };

            TwoFactorAuthService.VerifyHotp(null, 0, null).ReturnsForAnyArgs(true);
            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsForAnyArgs(verificationInfo);
            VerificationRepo.InsertUserDeviceInfo(null).ReturnsForAnyArgs(new ResultInfo { IsOk = isOK });

            var result = service.IsValidOtp(param);

            VerificationRepo.ReceivedWithAnyArgs().InsertUserDeviceInfo(null);
            Assert.True(result);
        }

        [Fact]
        public void IsInvalidOtp_WhenLoginOkButUseInvalidOtp_NotInsertUserDeviceToRepo()
        {
            var service = CreateService();
            var param = CreateVerificationOtpParam();
            var authMsg = param.LoginInfo + param.HashKey + param.Captcha + param.OtpCode;
            param.HashMessage = CryptographyHelper.SHA256Hash(authMsg, param.HashKey);
            var verificationInfo = new VerificationInfo
            {
                DeviceCount = 1,
                FailedLoginCount = null
            };

            TwoFactorAuthService.VerifyHotp(null, 0, null).ReturnsForAnyArgs(false);
            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsForAnyArgs(verificationInfo);

            var result = service.IsValidOtp(param);

            VerificationRepo.DidNotReceiveWithAnyArgs().InsertUserDeviceInfo(null);
            Assert.False(result);
        }

        [Fact]
        public void CreateActionLink_WhenCall_ReturnCorrectValue()
        {
            var service = CreateService();
            var nowTick = 10_000_000;
            var expirationMin = 10;
            var queryString = "controller=VerificationAccount&action=VerifyAccount&t=" + nowTick + "&e=" + expirationMin;
            var link = ConverterUtils.Base64Encode(SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, "abc", "12233333"));
            WrapperUtility.GetNowDateTime().ReturnsForAnyArgs(new DateTime(nowTick));

            var result = service.CreateActionLink("abc", "12233333", expirationMin);

            Assert.Equal(link, result.Link);
            Assert.Equal(nowTick.ToString(), result.TimeToken);

        }

        private static VerificationOtpParam CreateVerificationOtpParam()
            => new VerificationOtpParam
            {
                AffId = 1,
                Captcha = "1234",
                Counter = 1,
                DeviceInfo = "device",
                HashKey = "1234",
                HashMessage = "1234",
                IpAddress = "127.0.0.1",
                Language = "vi-VN",
                LoginInfo = "abc",
                OtpCode = "12345",
                SecretKey = "12345",
                UserAgent = "chrome"
            };

        private UserInfo CreateUserInfo()
            => new UserInfo
            {
                AffId = 1,
                Email = "abc@gamail.com",
                FirstName = "first",
                LastName = "last",
                MailOption = false,
                PreferredLanguage = "en-UK",
                SecretKey = "1234",
                UserName = "username"
            };

        private SendCodeParam CreateSendCodeParam()
            => new SendCodeParam
            {
                AffId = 1,
                CurrentLanguage = "en",
                Email = "abc@gamail.com",
                IntervalInMinute = 0,
                SentCodeTime = 10000,
                VerificationEmailParam = new MetalModels.Email.VerificationAccountEmailParam()
            };

        private VerificationService CreateService()
            => new VerificationService(TwoFactorAuthService, AccountRepo, MailService, CryptoService, WrapperUtility, VerificationRepo);
    }
}
