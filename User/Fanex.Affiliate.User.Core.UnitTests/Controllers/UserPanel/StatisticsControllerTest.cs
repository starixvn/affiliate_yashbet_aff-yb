using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class StatisticsControllerTest
    {
        [Fact]
        public void StatisticsDetailBySite_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            var param = new StatisticsDetailBySiteParam
            {
                TotalPages = 10
            };
            var trackingStatisticsList = new List<ReferrerStatistics> { new ReferrerStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetStatisticsDetailBySite(param)).Returns(trackingStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var expectedViewModel = new ReferrerStatisticsViewModel
            {
                DataList = trackingStatisticsList,
                IsDetail = true
            };

            // Action
            var viewResult = controller.StatisticsDetailBySite(param) as PartialViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as ReferrerStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/StatisticsPartial/_ReferrersStatisticsContent.cshtml", viewName);
            Assert.Equal(expectedViewModel.DataList, model.DataList);
            Assert.True(model.IsDetail);
        }

        private static StatisticsController CreateController(
            ISessionService sessionService = null,
            ILanguageService languageService = null,
            IStatisticsRepository staticticsRepository = null)
        {
            var appSetting = Mock.Of<IAppSetting>();
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeStaticticsRepository = staticticsRepository ?? (new Mock<IStatisticsRepository>()).Object;

            return new StatisticsController(appSetting, fakeSessionService, fakeLanguageService, fakeStaticticsRepository);
        }
    }
}
