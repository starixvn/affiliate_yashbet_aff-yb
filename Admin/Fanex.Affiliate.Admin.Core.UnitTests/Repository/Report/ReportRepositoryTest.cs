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
    public class ReportRepositoryTest
    {
        private const string _sp_GetMainReportData = "Aff_Admin_AffiliateReport_Sel";
        private const string _sp_GetDetailReportData = "Aff_Admin_AffiliateReport_Detail_Sel";

        [Fact]
        public void GetMainReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<AffiliateReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetReportData());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetMainReportData, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMainReportData(new AffiliateReportParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("", item.MonthDateString);
            Assert.Equal(123, item.TotalEarning);
        }

        [Fact]
        public void GetMainReportData_WhenCall_ReturnCorrectDataHasFullColumn()
        {
            // Arrange
            var repository = new Mock<AffiliateReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);
            dataReader.Setup(d => d.FieldCount).Returns(4);
            dataReader.Setup(d => d.GetName(0)).Returns("AffId");
            dataReader.Setup(d => d.GetName(1)).Returns("Username");
            dataReader.Setup(d => d.GetName(2)).Returns("MonthDate");
            dataReader.Setup(d => d.GetName(3)).Returns("Description");

            ReaderHelper.MapReaderData(dataReader, GetReportDataFullColumn());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetMainReportData, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMainReportData(new AffiliateReportParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Empty(item.MonthDateString);
            Assert.Equal(123, item.TotalEarning);
        }

        [Fact]
        public void GetMainReportData_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<AffiliateReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb(_sp_GetMainReportData, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMainReportData(new AffiliateReportParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void GetDetailReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<AffiliateReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetReportDataFullColumn());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetDetailReportData, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDetailReportData(new AffiliateReportParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("11/2019", item.MonthDateString);
        }

        [Fact]
        public void GetDetailReportData_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<AffiliateReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb(_sp_GetDetailReportData, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDetailReportData(new AffiliateReportParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetReportData()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TotalClick"] = 123,
                ["UniqueClick"] = 123,
                ["Signup"] = 123,
                ["FDM"] = 123,
                ["ActivePlayer"] = 123,
                ["Deposit"] = 123,
                ["Withdrawal"] = 123,
                ["Turnover"] = 123,
                ["Ggr"] = 123,
                ["Bonus"] = 123,
                ["PlatformFee"] = 123,
                ["BankingFee"] = 123,
                ["TotalFee"] = 123,
                ["CommissionRate"] = 123,
                ["Earning"] = 123,
                ["AffWithdrawal"] = 123,
                ["AffAdjustment"] = 123,
                ["BFF"] = 123,
                ["NoOfSubAff"] = 0,
                ["SubAffEarning"] = 0
            };
        }

        private IDictionary<string, object> GetReportDataFullColumn()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 1271808,
                ["Username"] = "B2CAFF226",
                ["MonthDate"] = new DateTime(2019, 11, 24),
                ["Description"] = "abc",
                ["TotalClick"] = 123,
                ["UniqueClick"] = 123,
                ["Signup"] = 123,
                ["FDM"] = 123,
                ["ActivePlayer"] = 123,
                ["Deposit"] = 123,
                ["Withdrawal"] = 123,
                ["Turnover"] = 123,
                ["Ggr"] = 123,
                ["Bonus"] = 123,
                ["PlatformFee"] = 123,
                ["BankingFee"] = 123,
                ["TotalFee"] = 123,
                ["CommissionRate"] = 123,
                ["Earning"] = 123,
                ["AffWithdrawal"] = 123,
                ["AffAdjustment"] = 123,
                ["BFF"] = 123,
                ["NoOfSubAff"] = 0,
                ["SubAffEarning"] = 0
            };
        }
    }
}
