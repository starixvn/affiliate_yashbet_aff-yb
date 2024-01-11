using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class StatementHistoryServiceTest
    {
        [Fact]
        public void GetSubAffiliate_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new SubAffiliateParam();
            var repository = new Mock<IStatementHistoryRepository>();
            repository.Setup(comm => comm.GetSubAffiliate(param)).Returns(new EntityResult<SubAffiliate> { MainData = new List<SubAffiliate> { new SubAffiliate() }, Total = new SubAffiliate() });
            var service = CreateStatementHistoryService(repository.Object);

            // Action
            var result = service.GetSubAffiliate(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetDailyEarning_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new DailyEarningParam();
            var repository = new Mock<IStatementHistoryRepository>();
            repository.Setup(comm => comm.GetDailyEarning(param)).Returns(new DailyEarningResult { MainData = new List<DailyEarning> { new DailyEarning() }, Total = new DailyEarning() });
            var service = CreateStatementHistoryService(repository.Object);

            // Action
            var result = service.GetDailyEarning(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetDailyEarningDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new DailyEarningParam();
            var repository = new Mock<IStatementHistoryRepository>();
            repository.Setup(comm => comm.GetDailyEarningDetail(param)).Returns(new List<DailyEarningDetail> { new DailyEarningDetail() });
            var service = CreateStatementHistoryService(repository.Object);

            // Action
            var result = service.GetDailyEarningDetail(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetStatementHistoryList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new AffiliatePagingByDateParam();
            var repository = new Mock<IStatementHistoryRepository>();
            repository.Setup(comm => comm.GetStatementHistoryList(param)).Returns(new EntityResult<StatementHistory> { MainData = new List<StatementHistory> { new StatementHistory() }, TotalItem = 1 });
            var service = CreateStatementHistoryService(repository.Object);

            // Action
            var result = service.GetStatementHistoryList(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetStatementHistoryList_WhenCall_ReturnCorrectDataHasTotal()
        {
            // Arrange
            var param = new AffiliatePagingByDateParam();
            var repository = new Mock<IStatementHistoryRepository>();
            repository.Setup(comm => comm.GetStatementHistoryList(param)).Returns(new EntityResult<StatementHistory>
            {
                MainData = new List<StatementHistory> {
                new StatementHistory { TransType = 201, Date = new DateTime(2020, 06, 10), Status = "Current", CalculatedType = 1 },
                new StatementHistory { TransType = 204, Date = new DateTime(2020, 06, 10), Status = "Current", CalculatedType = 1 },
                new StatementHistory { TransType = 201, Date = new DateTime(2020, 05, 10), Status = "Current", CalculatedType = 2 },
                new StatementHistory { TransType = 204, Date = new DateTime(2020, 05, 10), Status = "Current", CalculatedType = 2 },
                new StatementHistory { TransType = 201, Date = new DateTime(2020, 04, 10), Status = "Current", CalculatedType = 0 },
                new StatementHistory { TransType = 204, Date = new DateTime(2020, 04, 10), Status = "Current", CalculatedType = 0 },
                new StatementHistory { TransType = 204, Date = new DateTime(2020, 03, 10), Status = "Current" },
                new StatementHistory { TransType = 200, Date = new DateTime(2020, 06, 10), Status = "Cancelled", Description = "test" }
            },
                TotalItem = 5
            });
            var service = CreateStatementHistoryService(repository.Object);

            // Action
            var result = service.GetStatementHistoryList(param).MainData.ToList();

            // Assert
            Assert.Equal("2020-06-10", result[0].NormalizeDateText);
            Assert.Equal("Monthly Total", result[1].DescriptionDisplay);
            Assert.Equal("Uncalculated Month", result[0].DescriptionDisplay);
            Assert.Equal("Current Month Statistics", result[2].DescriptionDisplay);
            Assert.Equal("Current Month Statistics", result[3].DescriptionDisplay);
            Assert.Equal("test", result[4].DescriptionDisplay);
        }

        private static StatementHistoryService CreateStatementHistoryService(IStatementHistoryRepository statementHistoryRepository = null)
        {
            var fakeStatementHistoryRepository = statementHistoryRepository ?? Mock.Of<IStatementHistoryRepository>();

            return new StatementHistoryService(fakeStatementHistoryRepository);
        }
    }
}
