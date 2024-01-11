using Fanex.Affiliate.Admin.Core.MetaModels.ChangeAdminPassword;
using Fanex.Affiliate.Admin.Core.Services.ChangeAdminPassword;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.ChangeAdminPassword
{
    public class ChangeAdminPasswordServiceTest
    {
        [Fact]
        public void ChangePassword_WhenCall_ChangePasswordIsSuccessful()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null));
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("");

            var repository = new Mock<ChangeAdminPasswordService> { CallBase = true };
            repository.Setup(re => re.GetObjectDb("Aff_Admin_ChangePassword", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.ChangePassword(new ChangeAdminPasswordParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ChangePassword_WhenCall_ChangePasswordIsFailed()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null));
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("Old password is incorrect.");

            var repository = new Mock<ChangeAdminPasswordService> { CallBase = true };
            repository.Setup(re => re.GetObjectDb("Aff_Admin_ChangePassword", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.ChangePassword(new ChangeAdminPasswordParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Old password is incorrect.", result.Message);
        }
    }
}
