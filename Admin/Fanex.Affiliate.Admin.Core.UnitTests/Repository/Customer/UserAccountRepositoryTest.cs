using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Customer
{
    public class UserAccountRepositoryTest
    {
        [Fact]
        public void GetUserAccountInfo_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<UserAccountRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetUserAccountInfo());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetUserAccountInfo(1271808);

            // Assert
            Assert.Equal(1271808, result.AffId);
            Assert.True(result.HasWebSite);
        }

        [Fact]
        public void GetUserAccountInfo_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<UserAccountRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetUserAccountInfo(1271808);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateUserAccount_WhenCallWithInvalidParam_UpdateSuccessfully()
        {
            // Arrange
            var repository = new Mock<UserAccountRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateInfo_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateUserAccount(new UserAccountParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateUserAccount_WhenCallWithValidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update is failed.";
            var repository = new Mock<UserAccountRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateInfo_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateUserAccount(new UserAccountParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetUserAccountInfo()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 1271808,
                ["UserName"] = "B2CAFF226",
                ["FirstName"] = "Hinh",
                ["LastName"] = "Lê Văn",
                ["Gender"] = false,
                ["Email"] = "abc@gmail.com",
                ["Phone"] = "01234567899",
                ["ContactApps"] = "[{'Name':'MSN / YM / QQ','Value':'@123 Lê Văn','Order':0}]",
                ["Address"] = "Test",
                ["City"] = "test",
                ["State"] = "700000",
                ["Postal"] = "700000",
                ["Country"] = "Viet Nam",
                ["MoreSiteUrls"] = "www.sportsbookie.com",
                ["SiteUrl"] = "https://aff-yb.nexdev.net/YBUser/UserPanel/Main/#MyAccount",
                ["PreferredPayment"] = "Player",
                ["PlayerUsername"] = "bettingexp",
                ["NoSiteDesc"] = "test",
                ["NetellerAccountID"] = true,
                ["MoneybookersEmail"] = "abc@gmail.com",
                ["BankName"] = "Test",
                ["BankAddress"] = "test",
                ["BankSwiftCode"] = "test",
                ["BankAccountName"] = "test",
                ["BankAccountNo"] = "test",
                ["Question"] = "test",
                ["Answer"] = "test",
                ["MailOption"] = false
            };
        }
    }
}
