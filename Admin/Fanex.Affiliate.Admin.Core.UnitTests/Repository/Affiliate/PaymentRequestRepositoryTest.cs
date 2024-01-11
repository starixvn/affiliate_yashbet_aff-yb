using Fanex.Affiliate.Admin.Core.DTO.Payment;
using Fanex.Affiliate.Admin.Core.MetaModels.Payment;
using Fanex.Affiliate.Admin.Core.Repository.Payment;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Affiliate
{
    public class PaymentRequestRepositoryTest
    {
        [Fact]
        public void GetPaymentRequestList_WhenCall_ReturnCorrectEmailInfoList()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedPaymentRequestList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliatePaymentRequest_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPaymentRequestList(new PaymentRequestParam());
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("04/15/2020 00:00:00", item.TransDateText);
            Assert.Equal("04/15/2020 00:00:00", item.CompletedDateText);
        }

        [Fact]
        public void GetPaymentRequestList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliatePaymentRequest_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPaymentRequestList(new PaymentRequestParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void GetPaymentRequestDetail_WhenCall_ReturnCorrectEmailInfo()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedPaymentRequestDetail());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliatePaymentRequest_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPaymentRequestDetail(123);

            // Assert
            Assert.Equal(123, result.TransId);
            Assert.Equal("04/15/2020 00:00:00", result.TransDateText);
            Assert.Equal("04/15/2020 00:00:00", result.CompletedDateText);
        }

        [Fact]
        public void GetPaymentRequestDetail_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliatePaymentRequest_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPaymentRequestDetail(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdatePaymentRequest_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliatePaymentRequest_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdatePaymentRequest(new PaymentRequestUpdateParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void GetPaymentMethod_WhenCallWithInvalidAmount_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("MinAmount")).Returns(DBNull.Value);
            repository.Setup(re => re.GetObjectDb("Aff_PaymentMethod_Sel", 120)).Returns(objectDb.Object);

            // Action
            var (paymentMethods, minAmount) = repository.Object.GetPaymentMethod();

            // Assert
            objectDb.Verify(obj => obj.Query<PaymentMethodInfo>(null, null), Times.Once);
            Assert.Equal(0, minAmount);
        }

        [Fact]
        public void GetPaymentMethod_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<PaymentRequestRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("MinAmount")).Returns(100m);
            repository.Setup(re => re.GetObjectDb("Aff_PaymentMethod_Sel", 120)).Returns(objectDb.Object);

            // Action
            var (paymentMethods, minAmount) = repository.Object.GetPaymentMethod();

            // Assert
            objectDb.Verify(obj => obj.Query<PaymentMethodInfo>(null, null), Times.Once);
            Assert.Equal(100m, minAmount);
        }

        private IDictionary<string, object> GetExpectedPaymentRequestList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Username"] = "B2CAff226",
                ["Transid"] = 123,
                ["Status"] = "Processing",
                ["Amount"] = 100m,
                ["Transdate"] = new DateTime(2020, 04, 15),
                ["Completeddate"] = new DateTime(2020, 04, 15),
                ["Creator"] = "abc"
            };
        }

        private IDictionary<string, object> GetExpectedPaymentRequestDetail()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffName"] = "N88AFF001",
                ["Transid"] = 123,
                ["Status"] = "Processing",
                ["Amount"] = 100m,
                ["TransAmount"] = 100m,
                ["Transdate"] = new DateTime(2020, 04, 15),
                ["Completeddate"] = new DateTime(2020, 04, 15),
                ["Creator"] = "abc",
                ["email"] = "abc@gmail.com",
                ["AffId"] = 1271808,
                ["firstname"] = "abc",
                ["lastname"] = "123",
                ["PreferredPayment"] = "vi-VN",
                ["TransCurrencyCode"] = "UUS",
                ["PlayerUsername"] = "abc123",
                ["AdminDesc"] = "",
                ["AffDesc"] = ""
            };
        }
    }
}
