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
    public class AffTypeServiceTest
    {
        private const string SpKeyAffTypeSel = "Aff_AffTypeSel";

        [Fact]
        public void GetAffTypeList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AffTypeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetAfTypeList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyAffTypeSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetAffTypeList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetAffTypeList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<AffTypeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyAffTypeSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetAffTypeList();

            // Assert
            Assert.Empty(result);
        }

        private IDictionary<string, object> GetAfTypeList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TypeId"] = 123,
                ["TypeName"] = "Asian"
            };
        }
    }
}
