using Fanex.Affiliate.Admin.Core.Controllers.Report;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Report;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.ViewModels.Report;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Report
{
    public class TransferReportControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnCorrectViewAndModel()
        {
            //Arrange
            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository.Setup(c => c.GetBaseCurrencies()).Returns(new List<Currency> { new Currency() });
            var controller = CreateTransferReportController(null, currencyRepository.Object, null);

            //Action
            var result = controller.Index() as ViewResult;
            var data = result.Model as ReportCommonViewModel;

            //Assert
            Assert.Single(data.BaseCurrencyList);
            Assert.Equal("~/Areas/Portal/Views/Report/TransferReport.cshtml", result.ViewName);
        }

        [Fact]
        public void GetTransferReportData_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            //Arrange
            var param = new TransferReportParam();
            var controller = CreateTransferReportController();
            controller.ModelState.AddModelError("Error", string.Empty);

            //Action
            var result = controller.GetTransferReportData(param) as JsonNetResult;
            var data = result.Data as EntityResult<TransferReport>;

            //Assert
            Assert.Empty(data.MainData);
        }

        [Fact]
        public void GetTransferReportData_WhenCallWithValidParam_ReturnCorrectData()
        {
            //Arrange
            var param = new TransferReportParam();
            var TransferReportService = new Mock<ITransferReportService>();
            TransferReportService.Setup(p => p.GetTransferReportData(param)).Returns(new EntityResult<TransferReport> { MainData = new List<TransferReport> { new TransferReport() } });
            var controller = CreateTransferReportController(TransferReportService: TransferReportService.Object);

            //Action
            var result = controller.GetTransferReportData(param) as JsonNetResult;
            var data = result.Data as EntityResult<TransferReport>;

            //Assert
            Assert.Single(data.MainData);
        }

        public TransferReportController CreateTransferReportController(
            ISessionService sessionService = null,
            ICurrencyRepository currencyRepository = null,
            ITransferReportService TransferReportService = null)
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeCurrencyRepository = currencyRepository ?? Mock.Of<ICurrencyRepository>();
            var fakeTransferReportService = TransferReportService ?? Mock.Of<ITransferReportService>();

            return new TransferReportController(fakeSessionService, fakeCurrencyRepository, fakeTransferReportService);
        }
    }
}
