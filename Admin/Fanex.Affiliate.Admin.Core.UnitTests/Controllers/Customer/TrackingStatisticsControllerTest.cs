using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class TrackingStatisticsControllerTest
    {
        [Fact]
        public void GetTrackingStatisticsList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new TrackingStatisticsParam { AffId = 1271808, TrackingCode = "" };
            TrackingStatisticsParam passedParam = null;
            var trackingStatisticsService = new Mock<ITrackingStatisticsService>();
            trackingStatisticsService.Setup(s => s.GetTrackingStatisticsList(param)).Returns(new List<TrackingStatistics> { new TrackingStatistics() }).Callback<TrackingStatisticsParam>(p => passedParam = p);
            var controller = CreateController(trackingStatisticsService: trackingStatisticsService.Object);

            // Action
            var result = controller.GetTrackingStatisticsList(param) as JsonNetResult;
            var data = result.Data as List<TrackingStatistics>;

            // Assert
            Assert.Single(data);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetTrackingStatisticsList_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetTrackingStatisticsList(new TrackingStatisticsParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetTrackingStatisticsDetailList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new TrackingStatisticsDetailParam { AffId = 1271808, TrackingCode = "" };
            TrackingStatisticsDetailParam passedParam = null;
            var trackingStatisticsService = new Mock<ITrackingStatisticsService>();
            trackingStatisticsService.Setup(s => s.GetTrackingStatisticsDetailList(param)).Returns(new List<TrackingStatistics> { new TrackingStatistics() }).Callback<TrackingStatisticsDetailParam>(p => passedParam = p);
            var controller = CreateController(trackingStatisticsService: trackingStatisticsService.Object);

            // Action
            var result = controller.GetTrackingStatisticsDetailList(param) as JsonNetResult;
            var data = result.Data as List<TrackingStatistics>;

            // Assert
            Assert.Single(data);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetTrackingStatisticsDetailList_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetTrackingStatisticsDetailList(new TrackingStatisticsDetailParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private TrackingStatisticsController CreateController(
            ISessionService sessionService = null,
            ITrackingStatisticsService trackingStatisticsService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeTrackingStatisticsService = trackingStatisticsService ?? Mock.Of<ITrackingStatisticsService>();

            return new TrackingStatisticsController(fakeSessionService, fakeTrackingStatisticsService);
        }
    }
}
