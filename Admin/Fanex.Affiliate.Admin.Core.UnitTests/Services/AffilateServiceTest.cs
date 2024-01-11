using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services
{
    public class AffilateServiceTest
    {
        [Fact]
        public void GetAffiliateInfo_WhenCall_ReturnCorrectAfilliateInfo()
        {
            // Arrange
            var service = new Mock<AffiliateService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedAffiliateInfo());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetAffiliateBasicInfo(1271808);

            // Assert
            Assert.Equal(1271808, result.AffId);
        }

        [Fact]
        public void GetAffiliateInfo_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<AffiliateService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetAffiliateBasicInfo(1271808);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ApproveAffiliate_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var service = new Mock<AffiliateService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            objectDb.Setup(obj => obj.GetParameterValue("AffCode")).Returns("12345");
            service.Setup(re => re.GetObjectDb("Aff_Admin_Affiliate_Approved", 120)).Returns(objectDb.Object);

            // Action
            var (result, affCode) = service.Object.ApproveAffiliate(new ApproveAffiliateParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private IDictionary<string, object> GetExpectedAffiliateInfo()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 1271808,
                ["AffCode"] = "123456",
                ["GroupId"] = 123,
                ["UserName"] = "N88Aff002",
                ["FirstName"] = "abc",
                ["Email"] = "abc@gmail.com",
                ["MailOption"] = true,
                ["PreferredLanguage"] = "vi-VN",
                ["PlayerUsername"] = "123abc",
                ["PlayerCustId"] = 123,
                ["ParentAffName"] = "N88Aff001",
                ["AffType"] = 2,
                ["IsPrivate"] = false
            };
        }
    }
}
