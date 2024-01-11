using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class AuthServiceTest
    {
        private readonly IAuthRepository AuthRepo;
        private readonly ICryptographyService CryptographyService;
        private readonly IAppSetting AppSetting;
        private readonly IVerificationRepository VerificationRepo;

        const string Password = "Password";
        const string UserName = "aff123";

        private static string LoginQueryString => $"u={UserName}&p={Password}";

        public AuthServiceTest()
        {
            AuthRepo = Substitute.For<IAuthRepository>();
            CryptographyService = Substitute.For<ICryptographyService>();
            AppSetting = Substitute.For<IAppSetting>();
            VerificationRepo = Substitute.For<IVerificationRepository>();
        }

        [Fact]
        public void GetLoginResult_WhenLoginWithInternalAdminUserWithNotExist_ReturnErrorResult()
        {
            var param = CreateLoginParam();
            var service = CreateService();
            AuthRepo.GetUserInfo(Arg.Compat.Any<string>(), true).ReturnsNull();

            var result = service.GetLoginResult(param, true);

            Assert.False(result.Result.IsOk);
            Assert.Equal(Error.LoginWrongInfo, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenLoginWithInternalSuspendedAfUser_ReturnOK()
        {
            var param = CreateLoginParam();
            var userInfo = CreateSuspendedUserInfo();
            var service = CreateService();
            AuthRepo.GetUserInfo(Arg.Compat.Any<string>(), true).Returns(userInfo);

            var result = service.GetLoginResult(param, true);

            Assert.True(result.Result.IsOk);
        }

        [Fact]
        public void GetLoginResult_WhenLoginInternalSuccess_ReturnOk()
        {
            var param = CreateLoginParam();
            var userInfo = CreateAutherizedUserInfo();
            var service = CreateService();
            AuthRepo.GetUserInfo(Arg.Compat.Any<string>(), true).Returns(userInfo);

            var result = service.GetLoginResult(param, true);

            Assert.True(result.Result.IsOk);
            Assert.Equal(userInfo, result.UserInfo);
        }

        [Fact]
        public void GetLoginResult_WhenLoginWithInvalidTimeStamp_ReturnFailResult()
        {
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            var nowTick = DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeMilliseconds();
            var loginQueryString = $"{LoginQueryString}&t={nowTick}";
            CryptographyService.RsaDecrypt(null, null).ReturnsForAnyArgs(loginQueryString);

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.Contains(Error.YourSessionHasExpired, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenQueryStringHaveEmptyUserName_ReturnFailResult()
        {
            var nowTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var queryString = $"u={string.Empty}&p={Password}&t={nowTick}";
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            CryptographyService.RsaDecrypt(null, null).ReturnsForAnyArgs(queryString);

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.Contains(Lang.ErrmsgEntryUsername, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenQueryStringHaveEmptyPassword_ReturnFailResult()
        {
            var nowTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var queryString = $"u={UserName}&p={string.Empty}&t={nowTick}";
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            CryptographyService.RsaDecrypt(null, null).ReturnsForAnyArgs(queryString);

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.Contains(Lang.ErrmsgEntryPassword, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenUserIsSuspended_ReturnFailResult()
        {
            var service = CreateService();
            var param = CreateLoginParam();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var user = CreateSuspendedUserInfo();
            AuthRepo.GetUserInfo(null, false).ReturnsForAnyArgs(user);

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.Contains(Error.YourAccountHasBeenSuspended, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenUserVerifiedInfoNotExist_ReturnFailResult()
        {
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            param.IsVerifyingAccount = false;
            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsNullForAnyArgs();

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.True(result.Result.ShouldVerifyAccount);
        }

        [Fact]
        public void GetLoginResult_WhenUserVerifyFail_ReturnFailResult()
        {
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            param.IsVerifyingAccount = false;
            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsForAnyArgs(CreateFailVerficationInfo());

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.True(result.Result.ShouldVerifyAccount);
        }

        [Fact]
        public void GetLoginResult_WhenNotMatchPassword_ReturnFailResult()
        {
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            param.IsVerifyingAccount = false;
            VerificationRepo.GetLoginInfoByUserDevice(0, null, null).ReturnsForAnyArgs(CreateSuccessfulVerficationInfo());
            CryptographyService.EncryptPassword(null).ReturnsForAnyArgs("abc");

            var result = service.GetLoginResult(param, false);

            Assert.False(result.Result.IsOk);
            Assert.Contains(Error.LoginWrongInfo, result.Result.Message);
        }

        [Fact]
        public void GetLoginResult_WhenLoginSuccess_ReturnOkResult()
        {
            var service = CreateService();
            SetUpLoginNormaiUserPassedAllCheckCondition();
            var param = CreateLoginParam();
            var result = service.GetLoginResult(param, false);

            Assert.True(result.Result.IsOk);
        }

        [Fact]
        public void GetOnUserId_WhenCall_RepoCalled()
        {
            var service = CreateService();

            service.GetOnUserId(new OnUserIdParam());

            AuthRepo.ReceivedWithAnyArgs().GetOnUserId(null);
        }

        [Fact]
        public void GetSessionId_WhenCall_RepoCalled()
        {
            var service = CreateService();

            service.GetSessionId(1);

            AuthRepo.ReceivedWithAnyArgs().GetSessionId(1);
        }

        [Fact]
        public void ReCheckin_WhenCall_RepoCalled()
        {
            var service = CreateService();

            service.Recheckin(1, false);

            AuthRepo.ReceivedWithAnyArgs().Recheckin(1, false);
        }

        private void SetUpLoginNormaiUserPassedAllCheckCondition()
        {
            var nowTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var loginQueryString = $"{LoginQueryString}&t={nowTick}";
            CryptographyService.RsaDecrypt(null, null).ReturnsForAnyArgs(loginQueryString);
            AppSetting.LoginTimeout.Returns(30);
            AuthRepo.GetUserInfo(null, false).ReturnsForAnyArgs(CreateAutherizedUserInfo());
            CryptographyService.EncryptPassword(null).ReturnsForAnyArgs("password");
            AuthRepo.UpdateLoginResult(null).ReturnsForAnyArgs(new DTO.LoginResultInfo
            {
                IsOk = true
            });
        }

        private static VerificationInfo CreateFailVerficationInfo()
            => new VerificationInfo
            {
                DeviceCount = 11,
                FailedLoginCount = null
            };

        private static VerificationInfo CreateSuccessfulVerficationInfo()
            => new VerificationInfo
            {
                DeviceCount = 11,
                FailedLoginCount = 1
            };


        private static UserLoginParam CreateLoginParam()
            => new UserLoginParam
            {
                LoginInfo = "Chrome",
                IsVerifyingAccount = true,
                UserName = "test",
                Language = "vn-vi",
                LoginIp = "127.0.0.1",
                Password = "password",
                UserAgent = "Asia",
                ValidationKey = "1234"
            };

        private static AuthorizedUserInfo CreateAutherizedUserInfo()
            => new AuthorizedUserInfo
            {
                AffId = 1,
                AffType = "Asia",
                GroupName = "group",
                OnUserId = 1,
                Password = "password",
                PreferredLanguage = "vn-vi",
                Status = "Approved",
                UserName = "test"
            };

        private static AuthorizedUserInfo CreateSuspendedUserInfo()
            => new AuthorizedUserInfo
            {
                AffId = 1,
                AffType = "Asia",
                GroupName = "group",
                OnUserId = 1,
                Password = "password",
                PreferredLanguage = "vn-vi",
                Status = "Suspended",
                UserName = "test"
            };

        private AuthService CreateService()
            => new AuthService(AuthRepo, CryptographyService, AppSetting, VerificationRepo);
    }
}
