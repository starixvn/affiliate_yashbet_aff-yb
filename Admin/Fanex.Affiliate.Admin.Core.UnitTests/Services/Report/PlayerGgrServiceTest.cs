using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository.Report;
using Fanex.Affiliate.Admin.Core.Services.Report;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Report
{
    public class PlayerGgrServiceTest
    {
        [Fact]
        public void GetPlayerGgrReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new PlayerGgrReportParam { ProductIds = "123", FromDate = new DateTime(2021, 09, 01), ToDate = new DateTime(2021, 09, 29) };
            PlayerGgrReportParam passedParam = null;
            var repository = new Mock<IPlayerGgrReportRepository>();
            repository.Setup(re => re.GetPlayerGgrReportData(param)).Returns(new EntityResult<PlayerGgrReport> { MainData = new List<PlayerGgrReport> { new PlayerGgrReport() }, TotalItem = 1 }).Callback<PlayerGgrReportParam>(p => passedParam = p);
            var service = CreatePlayerGgrReportService(repository.Object);

            // Action
            var result = service.GetPlayerGgrReportData(param);

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetPlayerGgrReportData_WhenCallInvalidParam_ReturnEmpty()
        {
            // Arrange
            var service = CreatePlayerGgrReportService();

            // Action
            var result = service.GetPlayerGgrReportData(new PlayerGgrReportParam { FromDate = new DateTime(2020, 09, 01), ToDate = new DateTime(2021, 09, 29) });

            // Assert
            Assert.Empty(result.MainData);
        }

        private static PlayerGgrReportService CreatePlayerGgrReportService(IPlayerGgrReportRepository playerGgrReportRepository = null)
        {
            var fakePlayerGgrReportRepository = playerGgrReportRepository ?? Mock.Of<IPlayerGgrReportRepository>();

            return new PlayerGgrReportService(fakePlayerGgrReportRepository);
        }
    }
}
