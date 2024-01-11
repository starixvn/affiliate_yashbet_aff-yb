using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Data;
using NSubstitute;
using System;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository
{
    public class VerificationRepositoryTest : RepositoryTestBase<VerificationRepository>
    {
        [Fact]
        public void UpdateVerificationInfo_WhenDbNotThrowException_ReturnUpdateResult()
        {
            var spKey = "Aff_User_VerificationUpd";
            var erroCode = "0";
            var repo = CreateRepo();
            var param = CreateAuthResultParam();
            var db = Builder.Build(null, null);
            db.GetParameterValue("ErrCode").Returns(erroCode);

            var result = repo.UpdateVerificationInfo(param);

            Assert.Equal(spKey, db.SettingKey);
            AssertDbReceiveParam(db, param);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void GetLoginInfoByUserDevice_WhenDbReadNothing_ReturnNull()
        {
            const string SpKey = "Aff_User_VerificationInfoSel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            reader.Read().Returns(false);

            var result = repo.GetLoginInfoByUserDevice(1, null, null);

            Assert.Equal(SpKey, db.SettingKey);
            Assert.Null(result);
        }

        [Fact]
        public void GetLoginInfoByUSerDevice_WhenFailedLoginCountFieldNotExit_ReturnResultWithNullfailedLoginCount()
        {
            var verficationInfo = new VerificationInfo
            {
                DeviceCount = 1,
                FailedLoginCount = 2
            };
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            reader.Read().Returns(true);
            AssignObjectToReader(reader, verficationInfo);
            reader["FailedLoginCount"].Returns(DBNull.Value);

            var result = repo.GetLoginInfoByUserDevice(1, "127.0.0.1", "laptop");

            Assert.Null(result.FailedLoginCount);
        }

        [Fact]
        public void GetLoginInfoByUserDevice_WhenDbReturnCorrectResult_ReturnResult()
        {
            var verficationInfo = new VerificationInfo
            {
                DeviceCount = 1,
                FailedLoginCount = 2
            };
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            reader.Read().Returns(true);
            AssignObjectToReader(reader, verficationInfo);

            var result = repo.GetLoginInfoByUserDevice(1, "127.0.0.1", "laptop");

            Assert.Equal(verficationInfo.FailedLoginCount, result.FailedLoginCount);
            Assert.Equal(verficationInfo.DeviceCount, result.DeviceCount);
        }

        [Fact]
        public void InsertUserDeviceInfo_WhenDbNotThrowException_ReturnInsertResult()
        {
            const string SpKey = "Aff_User_VerificationSuccessIns";
            var erroCode = "0";
            var repo = CreateRepo();
            var param = CreateAuthResultParam();
            var db = Builder.Build(null, null);
            db.GetParameterValue("ErrCode").Returns(erroCode);

            var result = repo.InsertUserDeviceInfo(param);

            Assert.Equal(SpKey, db.SettingKey);
            AssertDbReceiveParam(db, param);
            Assert.True(result.IsOk);
        }

        private static void AssertDbReceiveParam(IObjectDb db, AuthResultBaseParam param)
        {
            db.Received().SetParameterValue("AffId", param.AffId);
            db.Received().SetParameterValue("IpAddress", param.IpAddress);
            db.Received().SetParameterValue("DeviceInfo", param.DeviceInfo);
            db.Received().SetParameterValue("Language", param.Language);

            db.Received().SetParameterValue("ErrCode", DefaultValue.Int);
            db.Received().SetParameterValue("ErrMsg", DefaultValue.String);
        }

        private AuthResultBaseParam CreateAuthResultParam()
            => new AuthResultBaseParam
            {
                AffId = 1,
                DeviceInfo = "mobile",
                IpAddress = "127.0.0.1",
                Language = "vi-VN"
            };
    }
}
