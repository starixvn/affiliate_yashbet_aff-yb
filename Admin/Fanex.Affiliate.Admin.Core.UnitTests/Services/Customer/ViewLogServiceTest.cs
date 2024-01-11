using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class ViewLogServiceTest
    {
        [Fact]
        public void GetLogTypeList_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<ViewLogService> { CallBase = true };
            repository.Setup(r => r.GetObjectDb("Aff_Admin_LogType_Sel", It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetLogTypeList();

            // Assert
            objectDb.Verify(obj => obj.Query<LogType>(null, null), Times.Once);
        }

        [Fact]
        public void GetViewLogList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<ViewLogService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetViewLogList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateLog_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetViewLogList(new ViewLogParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("12/27/2019 00:00:00", item.DateText);
        }

        [Fact]
        public void GetViewLogList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<ViewLogService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateLog_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetViewLogList(new ViewLogParam());

            // Assert
            Assert.Equal(0, result.TotalItem);
        }

        private IDictionary<string, object> GetViewLogList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["LogId"] = 123,
                ["UpdatedName"] = "sa",
                ["Date"] = new DateTime(2019, 12, 27),
                ["Action"] = "abc",
                ["Description"] = "abc",
                ["IP"] = "::1"
            };
        }
    }
}
