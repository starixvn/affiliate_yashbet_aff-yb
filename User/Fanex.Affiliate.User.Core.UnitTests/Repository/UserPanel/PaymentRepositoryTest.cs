using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class PaymentRepositoryTest : RepositoryTestBase<PaymentRepository>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPaymentList_WhenDbNotExistRecord_ReturnDefaultValue(bool isInternal)
        {
            const string uSpKey = "Aff_User_PaymentRequest_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var param = new PaymentParam { };
            param.IsInternal = isInternal;
            var db = Builder.Build(uSpKey, dbMode);
            var reader = db.ExecuteReader();
            reader.Read().Returns(true, false);
            reader[""].ReturnsForAnyArgs(DBNull.Value);
            db.GetParameterValue(null).ReturnsForAnyArgs(DBNull.Value);
            var repo = CreateRepo();

            var result = repo.GetPaymentList(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
            Assert.Equal(string.Empty, param.PreferredPayment);
            Assert.Equal(0m, param.BalBroughtForward);
            Assert.Equal(0m, param.MinAmount);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPaymentList_WhenDbExistRecord_ReturnPaymentList(bool isInternal)
        {
            const string uSpKey = "Aff_User_PaymentRequest_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var param = new PaymentParam { };
            param.IsInternal = isInternal;
            var db = Builder.Build(uSpKey, dbMode);
            var reader = db.ExecuteReader();
            reader.Read().Returns(true, true, false);
            reader[""].ReturnsForAnyArgs(DBNull.Value);
            db.GetParameterValue("BalBroughtForward").Returns("100");
            db.GetParameterValue("PreferredPayment").Returns("acc");
            db.GetParameterValue("MinAmount").Returns("100");
            var repo = CreateRepo();

            var result = repo.GetPaymentList(param);

            Assert.Equal(key, db.SettingKey);
            Assert.Equal(2, result.Count());
            Assert.Equal("acc", param.PreferredPayment);
            Assert.Equal(100, param.BalBroughtForward);
            Assert.Equal(100, param.MinAmount);
        }

        [Fact]
        public void CancelPaymentRequest_WhenGetNoErrorCode_ReturnSuccess()
        {
            const string uSpKey = "Aff_User_PaymentRequest_Upd";
            var param = new PaymentCancelParam { ErrCode = 101 };
            var db = Builder.Build(uSpKey);
            db.When(d => d.ExecuteNonQuery(param))
                .Do(c => (c.Args()[0] as PaymentCancelParam).ErrCode = 0);
            var repo = CreateRepo();

            var result = repo.CancelPaymentRequest(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void CreatePaymentRequest_WhenGetNoErrorCode_ReturnSuccess()
        {
            const string uSpKey = "Aff_User_PaymentRequest_Ins";
            var param = new PaymentRequestInfo { ErrCode = 101 };
            var db = Builder.Build(uSpKey);
            db.When(d => d.ExecuteNonQuery(param))
                .Do(c => (c.Args()[0] as PaymentRequestInfo).ErrCode = 0);
            var repo = CreateRepo();

            var result = repo.CreatePaymentRequest(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.True(result.IsOk);
        }

        [Fact]
        public void GetPaymentMethods_WhenCall_ReturnCorrectDataHasMinAmount()
        {
            const string uSpKey = "Aff_PaymentMethod_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null);
            db.GetParameterValue("MinAmount").Returns(100);

            var (_, MinAmount) = repo.GetPaymentMethods();

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.Equal(100, MinAmount);
            db.Received().Query<PaymentMethodInfo>();
        }

        [Fact]
        public void GetPaymentMethods_WhenCall_ReturnCorrectDataNotHasMinAmount()
        {
            const string uSpKey = "Aff_PaymentMethod_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null);
            db.GetParameterValue("MinAmount").Returns(null);

            var (_, MinAmount) = repo.GetPaymentMethods();

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.Equal(0, MinAmount);
            db.Received().Query<PaymentMethodInfo>();
        }
    }
}
