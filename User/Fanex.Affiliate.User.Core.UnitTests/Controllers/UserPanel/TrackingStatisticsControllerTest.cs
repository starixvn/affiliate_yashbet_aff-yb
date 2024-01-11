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
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class TrackingStatisticsControllerTest
    {
        [Fact]
        public void IndexGet_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            const bool IsInternal = false;
            var trackingCodeList = new List<string> { "[None]", "Code123" };
            var trackingStatisticsList = new List<TrackingStatistics> { new TrackingStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetTrackingCode(Constant.DefaultAffId, IsInternal)).Returns(trackingCodeList);
            statisticsRepository.Setup(s => s.GetTrackingStatistics(Constant.DefaultAffId, null, IsInternal)).Returns(trackingStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var expectedViewModel = new TrackingStatisticsViewModel
            {
                TrackingCodeList = new List<string> { "Code123" },
                TrackingStatisticsList = trackingStatisticsList,
                IsDetail = false
            };

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as TrackingStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/TrackingStatistics/Index.cshtml", viewName);
            Assert.Equal(expectedViewModel.TrackingCodeList.First(), model.TrackingCodeList.First());
            Assert.Single(model.TrackingCodeList);
            Assert.Equal(expectedViewModel.TrackingStatisticsList, model.TrackingStatisticsList);
            Assert.False(model.IsDetail);
        }

        [Fact]
        public void IndexPost_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            const string TrackingCode = "123";
            const bool IsInternal = false;
            var trackingStatisticsList = new List<TrackingStatistics> { new TrackingStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetTrackingStatistics(Constant.DefaultAffId, TrackingCode, IsInternal)).Returns(trackingStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var expectedViewModel = new TrackingStatisticsViewModel
            {
                TrackingStatisticsList = trackingStatisticsList,
                IsDetail = false
            };

            // Action
            var viewResult = controller.Index(TrackingCode) as PartialViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as TrackingStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/StatisticsPartial/_TrackingStatisticsContent.cshtml", viewName);
            Assert.Equal(expectedViewModel.TrackingStatisticsList, model.TrackingStatisticsList);
            Assert.False(model.IsDetail);
        }

        [Fact]
        public void TrackingStatisticsDetail_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            const string TrackingCode = "123";
            var param = new TrackingStatisticsDetailParam
            {
                TrackingCode = TrackingCode,
                DateMonth = new System.DateTime(2018, 12, 25)
            };
            var trackingStatisticsList = new List<TrackingStatistics> { new TrackingStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetTrackingStatisticsDetail(param)).Returns(trackingStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var expectedViewModel = new TrackingStatisticsViewModel
            {
                TrackingStatisticsList = trackingStatisticsList,
                IsDetail = true
            };

            // Action
            var viewResult = controller.TrackingStatisticsDetail(param) as PartialViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as TrackingStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/StatisticsPartial/_TrackingStatisticsContent.cshtml", viewName);
            Assert.Equal(expectedViewModel.TrackingStatisticsList, model.TrackingStatisticsList);
            Assert.Equal(12, param.Month);
            Assert.Equal(2018, param.Year);
            Assert.True(model.IsDetail);
        }

        private static TrackingStatisticsController CreateController(
            ISessionService sessionService = null,
            ILanguageService languageService = null,
            IStatisticsRepository staticticsRepository = null)
        {
            var appSetting = Mock.Of<IAppSetting>();
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeStaticticsRepository = staticticsRepository ?? (new Mock<IStatisticsRepository>()).Object;

            return new TrackingStatisticsController(appSetting, fakeSessionService, fakeLanguageService, fakeStaticticsRepository);
        }
    }
}
