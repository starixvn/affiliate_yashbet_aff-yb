using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Data;
using NSubstitute;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository
{
    public class AuthRepositoryTest : RepositoryTestBase<AuthRepository>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetOnUseId_WhenCall_QueryOnUserId(bool isInternal)
        {
            var repo = CreateRepo();
            var param = new OnUserIdParam
            {
                CustId = 1,
                Ip = "127.0.0.1",
                IsInternal = isInternal,
                NetSessionId = "session",
                On_UserId = 0,
                ServerId = 2,
                SubAccId = 4
            };
            var queryUserId = 100;
            var db = Builder.Build(null, null);
            db.When(c => c.ExecuteNonQuery(Arg.Compat.Any<OnUserIdParam>()))
                .Do(c =>
                {
                    (c.Args()[0] as OnUserIdParam).On_UserId = queryUserId;
                });


            repo.GetOnUserId(param);

            Assert.Equal(queryUserId, param.On_UserId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUserInfo_WhenDdCantReadRecord_ReturnNull(bool isInternal)
        {
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            reader.Read().Returns(false);

            var userInfo = repo.GetUserInfo("username", isInternal);

            Assert.Null(userInfo);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetUserInfo_WhenDbReturnCorrectRecord_ReturnCorrectObject(bool isInternal)
        {
            var authUserInfo = new AuthorizedUserInfo
            {
                AffId = 1,
                AffCode = 12345,
                UserName = "Name",
                Password = "pass",
                GroupName = "group",
                AffType = "asia",
                Status = "Approved",
                PreferredLanguage = "vi-VN",
                IsMaxSubLevel = false
            };
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            reader.Read().Returns(true);
            reader["Dep_IsDeductedPercent"].Returns(true);
            reader["Wit_IsDeductedPercent"].Returns(true);
            AssignObjectToReader(reader, authUserInfo);

            var userInfo = repo.GetUserInfo("username", isInternal);

            Assert.NotNull(userInfo);
            Assert.Equal(authUserInfo.AffId, userInfo.AffId);
        }

        [Fact]
        public void UpdateLoginResult_WhenDbNotThrowException_ReturnTrue()
        {
            var param = new LoginResultParam
            {
                AffId = 1,
                DeviceInfo = "destop",
                ErrCode = 2,
                ErrMsg = null,
                IpAddress = "127.0.0.1",
                IsSuccessful = true,
                Language = "vi-VN"
            };
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.GetParameterValue("ErrCode").Returns("0");

            var result = repo.UpdateLoginResult(param);

            AssertDbReciveParam(db, param);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void GetSessionId_WhenCall_ReturnSessionId()
        {
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var sessionId = "12345";
            var spKey = "Aff_SessionIdSel";
            db.ExecuteScalar().Returns(sessionId);

            var result = repo.GetSessionId(1);

            Assert.Equal(spKey, db.SettingKey);
            db.Received().SetParameterValue("OnUserId", 1);
            Assert.Equal(sessionId, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RecheckIn_WhenCall_ReturnKickOutStatus(bool isInternal)
        {
            var spSurfix = isInternal ? DBMode.View : DBMode.Main;
            var spKey = "Aff_Recheckin" + spSurfix;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var isKickOut = false;
            db.When(d => d.ExecuteNonQuery(Arg.Compat.Any<object>()))
                .Do(c => SetObjectPropetyValue(c.Args()[0], "IsKickedOut", isKickOut));

            var result = repo.Recheckin(1, isInternal);

            Assert.Equal(spKey, db.SettingKey);
            Assert.Equal(isKickOut, result);
        }

        private static void AssertDbReciveParam(IObjectDb db, LoginResultParam param)
        {
            db.Received().SetParameterValue("AffId", param.AffId);
            db.Received().SetParameterValue("IpAddress", param.IpAddress);
            db.Received().SetParameterValue("DeviceInfo", param.DeviceInfo);
            db.Received().SetParameterValue("Language", param.Language);
            db.Received().SetParameterValue("IsSuccessful", param.IsSuccessful);

            db.Received().SetParameterValue("ErrCode", DefaultValue.Int);
            db.Received().SetParameterValue("ErrMsg", DefaultValue.String);
        }
    }
}
