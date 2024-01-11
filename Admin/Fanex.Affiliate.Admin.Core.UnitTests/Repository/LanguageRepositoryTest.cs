using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository
{
    public class LanguageRepositoryTest
    {
        [Fact]
        public void GetLanguageList_WhenCall_ReturnCorrectLanguageList()
        {
            // Arrange
            var repository = new Mock<LanguageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedLanguageList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_Market_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLanguageList();

            // Assert
            Assert.Single(result);
        }

        private IDictionary<string, object> GetExpectedLanguageList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["MarketId"] = "en-US",
                ["MarketName"] = "English"
            };
        }
    }
}
