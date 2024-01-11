using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Customer
{
    public class StatementHistoryRepositoryTest
    {
        [Fact]
        public void GetStatementHistoryList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetStatementHistoryList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_StatementHistory_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetStatementHistoryList(new AffiliatePagingByDateParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("22-Nov-2019", item.DateText);
        }

        [Fact]
        public void GetStatementHistoryList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetStatementHistoryListIsTransaction());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_StatementHistory_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetStatementHistoryList(new AffiliatePagingByDateParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("Nov-2019", item.DateText);
        }

        [Fact]
        public void GetStatementHistoryList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_StatementHistory_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetStatementHistoryList(new AffiliatePagingByDateParam());

            // Assert
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public void GetSubAffiliate_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetSubAffiliateList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb("Aff_SubAffiliate_Commission_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetSubAffiliate(new SubAffiliateParam());

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetSubAffiliate_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetSubAffiliateListHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb("Aff_SubAffiliate_Commission_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetSubAffiliate(new SubAffiliateParam());

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetSubAffiliate_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb("Aff_SubAffiliate_Commission_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetSubAffiliate(new SubAffiliateParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void GetDailyEarning_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDailyEarningList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_DailyEarning_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDailyEarning(new DailyEarningParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("", item.Date);
        }

        [Fact]
        public void GetDailyEarning_WhenCall_ReturnCorrectDataHasWinlostDate()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);
            dataReader.Setup(d => d.FieldCount).Returns(1);
            dataReader.Setup(d => d.GetName(0)).Returns("WinlostDate");

            ReaderHelper.MapReaderData(dataReader, GetDailyEarningListHasWinlostDate());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_DailyEarning_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDailyEarning(new DailyEarningParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("22 November 2019", item.Date);
        }

        [Fact]
        public void GetDailyEarning_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_DailyEarning_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDailyEarning(new DailyEarningParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void GetDailyEarningDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDailyEarningDetailList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_DailyEarning_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDailyEarningDetail(new DailyEarningParam());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetDailyEarningDetail_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<StatementHistoryRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_DailyEarning_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDailyEarningDetail(new DailyEarningParam());

            // Assert
            Assert.Empty(result);
        }

        private IDictionary<string, object> GetStatementHistoryList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TransId"] = 123,
                ["Amount"] = 100,
                ["TransType"] = 123,
                ["MonthDate"] = new DateTime(2019, 11, 22),
                ["Status"] = "Approved",
                ["Creator"] = "sa",
                ["Description"] = "abc",
                ["CalculatedType"] = DBNull.Value
            };
        }

        private IDictionary<string, object> GetStatementHistoryListIsTransaction()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TransId"] = 123,
                ["Amount"] = 100,
                ["TransType"] = 0,
                ["MonthDate"] = new DateTime(2019, 11, 22),
                ["Status"] = "Approved",
                ["Creator"] = "sa",
                ["Description"] = "abc",
                ["CalculatedType"] = DBNull.Value
            };
        }

        private IDictionary<string, object> GetDailyEarningList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Deposit"] = 123,
                ["Withdrawal"] = 123,
                ["Ggr"] = 123,
                ["Bonus"] = 123,
                ["BankingFee"] = 123,
            };
        }

        private IDictionary<string, object> GetDailyEarningListHasWinlostDate()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Deposit"] = 123,
                ["Withdrawal"] = 123,
                ["Ggr"] = 123,
                ["Bonus"] = 123,
                ["BankingFee"] = DBNull.Value,
                ["WinlostDate"] = new DateTime(2019, 11, 22)
            };
        }

        private IDictionary<string, object> GetDailyEarningDetailList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustId"] = 123,
                ["UserName"] = "abc",
                ["IBCUsername"] = "abc",
                ["Ggr"] = 123,
                ["Deposit"] = 123,
                ["Withdrawal"] = 123,
                ["Bonus"] = 123,
                ["BankingFee"] = 123,
            };
        }

        private IDictionary<string, object> GetSubAffiliateList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 123,
                ["AffName"] = "abc",
                ["Commission"] = 123,
                ["SubAffCommission"] = 123
            };
        }

        private IDictionary<string, object> GetSubAffiliateListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["AffId"] = 123,
                ["AffName"] = "abc",
                ["Commission"] = DBNull.Value,
                ["SubAffCommission"] = DBNull.Value
            };
        }
    }
}
