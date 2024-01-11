using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Data;
using Moq;
using System;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class AdjustmentServiceTest
    {
        [Fact]
        public void GetBalanceBroughtForward_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdjustmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.GetParameterValue("BalBroughtForward")).Returns(123);
            service.Setup(r => r.GetObjectDb("Aff_CustomerSelBalBF", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetBalanceBroughtForward(1271808);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void GetBalanceBroughtForward_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<AdjustmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.GetParameterValue("BalBroughtForward")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb("Aff_CustomerSelBalBF", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetBalanceBroughtForward(1271808);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<AdjustmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateAdjustment_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertAdjustment(new AdjustmentParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert Adjustment is failed.";
            var service = new Mock<AdjustmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateAdjustment_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertAdjustment(new AdjustmentParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }
    }
}
