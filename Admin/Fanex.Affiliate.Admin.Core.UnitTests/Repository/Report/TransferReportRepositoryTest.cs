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
    public class TransferReportRepositoryTest
    {
        private const string _sp_GetTransferReportDataSP = "Aff_TransferBalanceHistory_Sel";

        [Fact]
        public void GetTransferReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<TransferReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTransferReportData());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(_sp_GetTransferReportDataSP, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetTransferReportData(new TransferReportParam { TransId = "123" });
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("10/21/2021 00:00:00", item.CreatedDateText);
        }

        [Fact]
        public void GetTransferReportData_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<TransferReportRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb(_sp_GetTransferReportDataSP, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetTransferReportData(new TransferReportParam { TransId = "123" });

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetTransferReportData()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffName"] = "Test",
                ["AffPlayerName"] = "Test",
                ["PlayerName"] = "Test",
                ["TransTime"] = new DateTime(2021, 10, 21),
                ["TransId"] = 123,
                ["CurrencyCode"] = "USD",
                ["Amount"] = 123,
                ["BasedAmount"] = 123
            };
        }
    }
}
