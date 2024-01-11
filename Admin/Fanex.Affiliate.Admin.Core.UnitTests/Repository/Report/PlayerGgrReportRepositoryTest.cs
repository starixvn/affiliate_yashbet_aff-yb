using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository.Report;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Report
{
    public class PlayerGgrReportRepositoryTest
    {
        private const string _sp_GetPlayerGgrReportDataSP = "Aff_Admin_PlayerReport_GGR";

        [Fact]
        public void GetPlayerGgrReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<PlayerGgrReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetPlayerGgrReportData());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetPlayerGgrReportDataSP, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPlayerGgrReportData(new PlayerGgrReportParam { ProductIds = "1,2,3" });
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("09/19/2021 00:00:00", item.RegistrationDateText);
            Assert.Equal("09/19/2021 00:00:00", item.FdmDateText);
        }

        [Fact]
        public void GetPlayerGgrReportData_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var repository = new Mock<PlayerGgrReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetPlayerGgrReportDataHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetPlayerGgrReportDataSP, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPlayerGgrReportData(new PlayerGgrReportParam { ProductIds = "1,2,3" });
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Empty(item.RegistrationDateText);
            Assert.Empty(item.FdmDateText);
        }

        [Fact]
        public void GetPlayerGgrReportData_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<PlayerGgrReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb(_sp_GetPlayerGgrReportDataSP, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetPlayerGgrReportData(new PlayerGgrReportParam { ProductIds = "1,2,3" });

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetPlayerGgrReportData()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffUsername"] = "Test",
                ["Username"] = "Test",
                ["RegisteredDate"] = new DateTime(2021, 09, 19),
                ["FirstDepositDate"] = new DateTime(2021, 09, 19),
                ["Total_Turnover"] = 123,
                ["Total_Ggr"] = 123,
                ["1_Turnover"] = 123,
                ["1_Ggr"] = 123,
                ["2_Turnover"] = 123,
                ["2_Ggr"] = 123,
                ["3_Turnover"] = 123,
                ["3_Ggr"] = 123
            };
        }

        private IDictionary<string, object> GetPlayerGgrReportDataHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffUsername"] = "Test",
                ["Username"] = "Test",
                ["RegisteredDate"] = DBNull.Value,
                ["FirstDepositDate"] = DBNull.Value,
                ["Total_Turnover"] = 123,
                ["Total_Ggr"] = 123,
                ["1_Turnover"] = DBNull.Value,
                ["1_Ggr"] = DBNull.Value,
                ["2_Turnover"] = DBNull.Value,
                ["2_Ggr"] = DBNull.Value,
                ["3_Turnover"] = DBNull.Value,
                ["3_Ggr"] = DBNull.Value,
            };
        }
    }
}
