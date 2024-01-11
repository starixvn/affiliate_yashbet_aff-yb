using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;
using Fanex.Affiliate.Admin.Core.Repository.Affiliate;
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
    public class AffiliateInfoRepositoryTest
    {

        [Fact]
        public void GetAffiliateList_WhenCall_ReturnCorrectAfilliateInfo()
        {
            // Arrange
            var repository = new Mock<AffiliateInfoRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedAffiliateList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAffiliateList(new AffiliateInfoParam());
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("01/01/2020 00:00:00", item.RegistrationDateText);
            Assert.Equal("04/15/2020 00:00:00", item.ApprovalDateText);
            Assert.Equal("abc", item.ApprovedByText);
        }

        [Fact]
        public void GetAffiliateList_WhenCall_ReturnAffiliateHasNull()
        {
            // Arrange
            var repository = new Mock<AffiliateInfoRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedAffiliateListHasNull());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAffiliateList(new AffiliateInfoParam());
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("01/01/2020 00:00:00", item.RegistrationDateText);
            Assert.Equal("N/A", item.ApprovalDateText);
            Assert.Equal("N/A", item.ApprovedByText);
        }

        [Fact]
        public void GetAffiliateList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<AffiliateInfoRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAffiliateList(new AffiliateInfoParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void ToggleFavorite_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<AffiliateInfoRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Favorite_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.ToggleFavorite(new FavoriteParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ChangePassword_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<AffiliateInfoRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Password_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.ChangePassword(new PasswordParam { Password = "123abc", ConfirmPassword = "123abc" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private IDictionary<string, object> GetExpectedAffiliateList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 1271808,
                ["Status"] = "Approved",
                ["UserName"] = "N88aff001",
                ["FirstName"] = "abc",
                ["LastName"] = "123",
                ["PreferredLanguage"] = "vi-VN",
                ["Email"] = "abc@gmail.com",
                ["RegisteredDate"] = new DateTime(2020, 01, 01),
                ["ApprovedDate"] = new DateTime(2020, 04, 15),
                ["GroupName"] = "N88aff001",
                ["BFF"] = 123m,
                ["NoOfMember"] = 1,
                ["IsFavorite"] = true,
                ["IsClosed"] = false,
                ["Currency"] = "USD",
                ["PlayerCurrency"] = "USD",
                ["Phone"] = "0123456789",
                ["Player_Username"] = "abc123",
                ["IsPrivate"] = false,
                ["ParentAffId"] = 1,
                ["ParentAffName"] = "N88aff000",
                ["NoOfSubAff"] = 0,
                ["ApprovedBy"] = "abc"
            };
        }

        private IDictionary<string, object> GetExpectedAffiliateListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 1271808,
                ["Status"] = "Approved",
                ["UserName"] = "N88aff001",
                ["FirstName"] = "abc",
                ["LastName"] = "123",
                ["PreferredLanguage"] = "vi-VN",
                ["Email"] = "abc@gmail.com",
                ["RegisteredDate"] = new DateTime(2020, 01, 01),
                ["ApprovedDate"] = DBNull.Value,
                ["GroupName"] = "N88aff001",
                ["BFF"] = DBNull.Value,
                ["NoOfMember"] = DBNull.Value,
                ["IsFavorite"] = true,
                ["IsClosed"] = false,
                ["Currency"] = "USD",
                ["PlayerCurrency"] = "USD",
                ["Phone"] = "0123456789",
                ["Player_Username"] = "abc123",
                ["IsPrivate"] = false,
                ["ParentAffId"] = DBNull.Value,
                ["ParentAffName"] = DBNull.Value,
                ["NoOfSubAff"] = 0,
                ["ApprovedBy"] = DBNull.Value
            };
        }
    }
}
