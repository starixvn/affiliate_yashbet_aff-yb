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
    public class TransferServiceTest
    {
        [Fact]
        public void GetTransferReportData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new TransferReportParam { TransId = "123", FromDate = new DateTime(2021, 10, 01), ToDate = new DateTime(2021, 10, 21) };
            TransferReportParam passedParam = null;
            var repository = new Mock<ITransferReportRepository>();
            repository.Setup(re => re.GetTransferReportData(param)).Returns(new EntityResult<TransferReport> { MainData = new List<TransferReport> { new TransferReport() }, TotalItem = 1 }).Callback<TransferReportParam>(p => passedParam = p);
            var service = CreateTransferReportService(repository.Object);

            // Action
            var result = service.GetTransferReportData(param);

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetTransferReportData_WhenCallInvalidParam_ReturnEmpty()
        {
            // Arrange
            var service = CreateTransferReportService();

            // Action
            var result = service.GetTransferReportData(new TransferReportParam { FromDate = new DateTime(2021, 11, 11), ToDate = new DateTime(2021, 10, 21) });

            // Assert
            Assert.Empty(result.MainData);
        }

        private static TransferReportService CreateTransferReportService(ITransferReportRepository TransferReportRepository = null)
        {
            var fakeTransferReportRepository = TransferReportRepository ?? Mock.Of<ITransferReportRepository>();

            return new TransferReportService(fakeTransferReportRepository);
        }
    }
}
