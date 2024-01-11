using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class AccountRepositoryTest : RepositoryTestBase<AccountRepository>
    {
        [Fact]
        public void GetAccountInfo_WhenCall_QueryAccountInfo()
        {
            const string uSpKey = "Aff_User_UserInfoSel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.QueryEntity<AccountInformation>(null).ReturnsForAnyArgs(new AccountInformation());

            var result = repo.GetAccountInfo(1);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateAccountInformation_WhenCall_ReturnUpdateResult()
        {
            const string uSpKey = "Aff_User_UserInfoUpd";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var param = CreateUpdateAccountParam();
            db.When(d => d.ExecuteNonQuery(param))
                .Do(c => (c.Args()[0] as UpdateAccountParam).ErrCode = 0);

            var result = repo.UpdateAccountInformation(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void UpdatePassword_WhenCall_ReturnUpdateResult()
        {
            const string uSpKey = "Aff_AffiliateUpdPwd";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var param = CreateUpdatePasswordParam();
            db.When(d => d.ExecuteNonQuery(Arg.Compat.Any<object>()))
                .Do(c => SetObjectPropetyValue(c.Args()[0], "ErrCode", 0));

            var result = repo.UpdatePassword(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void GetUserInfoByEmail_WhenCall_QueryUserInfo()
        {
            const string uSpKey = "Aff_User_ForgotPassword";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var param = new UserInfoByEmailParam { Email = "", ErrCode = 1, Ip = "", Language = "", ValidCode = "" };
            db.QueryEntity<UserInfo>().ReturnsForAnyArgs(CreateUserInfo());

            var result = repo.GetUserInfoByEmail(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotNull(result);
        }

        [Fact]
        public void RecoverPassword_WhenCall_ReturnUserInfo()
        {
            const string uSpKey = "Aff_User_RecoverPassWord";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var param = new PasswordRecoveryParam { AffId = 1, ErrCode = 0, Ip = "", Language = "", NewPassword = "", ValidCode = "" };
            db.QueryEntity<UserInfo>().ReturnsForAnyArgs(CreateUserInfo());

            var result = repo.RecoverPassword(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUserInfoById_WhenCall_QueryUserInfo(bool isInternal)
        {
            const string uSpKey = "Aff_User_GeneralInfo_Sel";
            var mode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + mode;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.QueryEntity<UserInfo>().ReturnsForAnyArgs(CreateUserInfo());

            var result = repo.GetUserInfoById(1, isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotNull(result);
        }


        [Fact]
        public void CreateAffiliate_WhenReturnNoErrorCode_ReturnOkResult()
        {
            const string uSpKey = "Aff_User_Affiliate_Ins";
            var repo = CreateRepo();
            var param = new UserSignUpParam
            {
                Address = "",
                AffType = "",
                Answer = "",
                Captcha = "1234",
                City = "",
                ContactAppList = new List<ContactApp> { new ContactApp { Name = "zalo", Order = 1, Value = "abc" } },
                ContactApps = "",
                Country = "",
                Email = "",
                ErrCode = 101,
                ErrMsg = "error",
                FirstName = "name",
                Gender = true,
                HasWebSite = false,
                Ip = "",
                IsLocal = false,
                Language = "",
                LastName = "name",
                MoreSiteUrls = "",
                NoSiteDesc = "",
                Phone = "123456",
                Player_UserName = "av",
                Postal = "",
                PreferredLanguage = "",
                QuestionId = 1,
                SiteUrl = "",
                State = "",
                UserName = "username",
                VerifyEmail = "abc@gmail.com"
            };

            var db = Builder.Build(null, null);
            db.When(d => d.ExecuteNonQuery(param))
                .Do(c =>
                {
                    var arg = c.Args()[0];
                    SetObjectPropetyValue(arg, "ErrCode", 0);
                    SetObjectPropetyValue(arg, "ErrMsg", null);
                });

            var result = repo.CreateAffiliate(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.True(result.IsOk);
            Assert.Equal("Player", UserSignUpParam.PreferredPayment);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetommissionSettingList_WhenCall_QueryInfo(bool isInternal)
        {
            const string uSpKey = "Aff_User_CommSetting_Sel";
            var mode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + mode;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.Query<CommissionSetting>().ReturnsForAnyArgs(new List<CommissionSetting>
            {
                new CommissionSetting {Commission = 100, MaxAmount = 1000, MaxQm = 3, MinAmount =10, MinQm =1 , TierId=2 }
            });

            var result = repo.GetCommissionSettingList(1, isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void CreateSecretKey_WhenReceiveNoExistErrorCode_ReturnSuccessCode()
        {
            const string uSpKey = "Aff_User_SecretKeyIns";
            const int successCode = 0;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.GetParameterValue("ErrCode").Returns(DBNull.Value);

            var result = repo.CreateSecretKey(1, "");

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.Equal(successCode, result);
        }

        [Fact]
        public void CreateSecretKey_WhenReceiveErrorCode_ReturnErrorCode()
        {
            const string uSpKey = "Aff_User_SecretKeyIns";
            const int errorCode = 100;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.GetParameterValue("ErrCode").Returns(errorCode);

            var result = repo.CreateSecretKey(1, "");

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.Equal(errorCode, result);
        }

        [Fact]
        public void GetUserInfoById_WhenCall_CallQuery()
        {
            const string uSpKey = "Aff_User_GeneralInfo_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetUserInfoById(1, false);

            Assert.Equal(uSpKey, db.SettingKey);
            db.Received().QueryEntity<UserInfo>(Arg.Any<object>());
        }

        [Fact]
        public void GetGeneralAffInfo_WhenCall_CallQuery()
        {
            const string uSpKey = "Aff_Get_GeneralAffInfo";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetGeneralAffInfo(1, null);

            Assert.Equal(uSpKey, db.SettingKey);
            db.Received().QueryEntity<GeneralAffInfo>(Arg.Any<object>());
        }

        private static UpdateAccountParam CreateUpdateAccountParam()
            => new UpdateAccountParam
            {
                AffId = 1,
                ContactApps = "",
                ErrCode = 1,
                ErrMsg = "",
                Ip = "127.0.0.1",
                Language = "vi-VN",
                MoreSiteUrls = "",
                NoSiteDesc = string.Empty,
                Phone = "123456",
                SiteUrl = "abc.com"
            };

        private static UpdatePasswordParam CreateUpdatePasswordParam()
            => new UpdatePasswordParam
            {
                AffId = 1,
                ConfirmNewPassword = "1234",
                ErrCode = 1,
                ErrMsg = "",
                Ip = "",
                Language = "",
                NewPassword = "1234",
                OldPassword = "ancd"
            };

        private static UserInfo CreateUserInfo()
            => new UserInfo
            {
                AffId = 1,
                Email = "abc@gmail.com",
                FirstName = "name",
                LastName = "last",
                MailOption = true,
                PreferredLanguage = "",
                SecretKey = "abc",
                UserName = "name"
            };
    }
}
