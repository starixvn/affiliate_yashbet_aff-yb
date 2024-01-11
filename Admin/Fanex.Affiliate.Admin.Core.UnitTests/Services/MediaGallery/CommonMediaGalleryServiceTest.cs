using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.MediaGallery
{
    public class CommonMediaGalleryServiceTest
    {
        private const string SpKeyBannerSelType = "Aff_Admin_MediaCategory_Sel";

        [Fact]
        public void GetCategoryList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<CommonMediaGalleryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetCategoryList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyBannerSelType, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCategoryList(1, 1);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetCategoryList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<CommonMediaGalleryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyBannerSelType, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCategoryList(1, 1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetCurrencyList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<CommonMediaGalleryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetCurrencyList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_MediaCurrency_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCurrencyList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetCurrencyList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<CommonMediaGalleryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_MediaCurrency_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCurrencyList();

            // Assert
            Assert.Empty(result);
        }

        private IDictionary<string, object> GetCategoryList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Category"] = "Casino",
                ["SubCategory"] = "test",
                ["TargetId"] = 1
            };
        }

        private IDictionary<string, object> GetCurrencyList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["CurrencyId"] = 123,
                ["Currency"] = "USD"
            };
        }
    }
}
