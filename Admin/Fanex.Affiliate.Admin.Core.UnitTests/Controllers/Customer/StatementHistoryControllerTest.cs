using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class StatementHistoryControllerTest
    {
        [Fact]
        public void GetStatementHistoryList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new AffiliatePagingByDateParam { AffId = 1271808, PageIndex = 1, PageSize = 20 };
            AffiliatePagingByDateParam passedParam = null;
            var statementHistoryService = new Mock<IStatementHistoryService>();
            statementHistoryService.Setup(s => s.GetStatementHistoryList(param)).Returns(new EntityResult<StatementHistory> { MainData = new List<StatementHistory> { new StatementHistory() }, TotalItem = 1 }).Callback<AffiliatePagingByDateParam>(p => passedParam = p);
            var controller = CreateStatementHistoryController(statementHistoryService: statementHistoryService.Object);

            // Action
            var result = controller.GetStatementHistoryList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<StatementHistory>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetStatementHistoryList_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateStatementHistoryController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetStatementHistoryList(new AffiliatePagingByDateParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetSubAffiliate_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new SubAffiliateParam { AffId = 1271808, PageIndex = 1, PageSize = 20 };
            SubAffiliateParam passedParam = null;
            var statementHistoryService = new Mock<IStatementHistoryService>();
            statementHistoryService.Setup(s => s.GetSubAffiliate(param)).Returns(new EntityResult<SubAffiliate> { MainData = new List<SubAffiliate> { new SubAffiliate() }, TotalItem = 1 }).Callback<SubAffiliateParam>(p => passedParam = p);
            var controller = CreateStatementHistoryController(statementHistoryService: statementHistoryService.Object);

            // Action
            var result = controller.GetSubAffiliate(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<SubAffiliate>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetSubAffiliate_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateStatementHistoryController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetSubAffiliate(new SubAffiliateParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetDailyEarning_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new DailyEarningParam { AffId = 1271808 };
            DailyEarningParam passedParam = null;
            var statementHistoryService = new Mock<IStatementHistoryService>();
            statementHistoryService.Setup(s => s.GetDailyEarning(param)).Returns(new DailyEarningResult { MainData = new List<DailyEarning> { new DailyEarning() }, Total = new DailyEarning() }).Callback<DailyEarningParam>(p => passedParam = p);
            var controller = CreateStatementHistoryController(statementHistoryService: statementHistoryService.Object);

            // Action
            var result = controller.GetDailyEarning(param) as JsonNetResult;
            var entityResult = result.Data as DailyEarningResult;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetDailyEarning_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateStatementHistoryController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetDailyEarning(new DailyEarningParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetDailyEarningDetail_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new DailyEarningParam { AffId = 1271808 };
            DailyEarningParam passedParam = null;
            var statementHistoryService = new Mock<IStatementHistoryService>();
            statementHistoryService.Setup(s => s.GetDailyEarningDetail(param)).Returns(new DailyEarningDetailResult { MainData = new List<DailyEarningDetail> { new DailyEarningDetail() }, Total = new DailyEarningDetail() }).Callback<DailyEarningParam>(p => passedParam = p);
            var controller = CreateStatementHistoryController(statementHistoryService: statementHistoryService.Object);

            // Action
            var result = controller.GetDailyEarningDetail(param) as JsonNetResult;
            var entityResult = result.Data as DailyEarningDetailResult;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetDailyEarningDetail_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateStatementHistoryController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetDailyEarningDetail(new DailyEarningParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private StatementHistoryController CreateStatementHistoryController(
            ISessionService sessionService = null,
            IStatementHistoryService statementHistoryService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeStatementHistoryService = statementHistoryService ?? Mock.Of<IStatementHistoryService>();

            return new StatementHistoryController(fakeSessionService, fakeStatementHistoryService);
        }
    }
}
