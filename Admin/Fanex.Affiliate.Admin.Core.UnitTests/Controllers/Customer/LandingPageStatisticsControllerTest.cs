using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class LandingPageStatisticsControllerTest
    {
        [Fact]
        public void GetLandingPageStatistics_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new LandingPageStatisticsParam();
            LandingPageStatisticsParam passedParam = null;
            var landingPageStatisticsRepository = new Mock<ILandingPageStatisticsRepository>();
            landingPageStatisticsRepository.Setup(t => t.GetLandingPageStatisticsList(param)).Returns(new List<LandingPageStatistics> { new LandingPageStatistics() }).Callback<LandingPageStatisticsParam>(p => passedParam = p);
            var controller = CreateLandingPageStatisticsController(landingPageStatisticsRepository: landingPageStatisticsRepository.Object);

            // Action
            var result = controller.GetLandingPageStatisticsList(param) as JsonNetResult;
            var entityResult = result.Data as IEnumerable<LandingPageStatistics>;

            // Assert
            Assert.Single(entityResult);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetLandingPageStatistics_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateLandingPageStatisticsController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetLandingPageStatisticsList(new LandingPageStatisticsParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetLandingPageStatisticsDetailList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new LandingPageStatisticsDetailParam();
            LandingPageStatisticsDetailParam passedParam = null;
            var landingPageStatisticsRepository = new Mock<ILandingPageStatisticsRepository>();
            landingPageStatisticsRepository.Setup(t => t.GetLandingPageStatisticsDetailList(param)).Returns(new EntityResult<LandingPageStatistics> { MainData = new List<LandingPageStatistics> { new LandingPageStatistics() }, TotalItem = 1 }).Callback<LandingPageStatisticsDetailParam>(p => passedParam = p);
            var controller = CreateLandingPageStatisticsController(landingPageStatisticsRepository: landingPageStatisticsRepository.Object);

            // Action
            var result = controller.GetLandingPageStatisticsDetailList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<LandingPageStatistics>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetLandingPageStatisticsDetailList_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateLandingPageStatisticsController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetLandingPageStatisticsDetailList(new LandingPageStatisticsDetailParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private LandingPageStatisticsController CreateLandingPageStatisticsController(
            ISessionService sessionService = null,
            ILandingPageStatisticsRepository landingPageStatisticsRepository = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLandingPageStatisticsRepository = landingPageStatisticsRepository ?? Mock.Of<ILandingPageStatisticsRepository>();

            return new LandingPageStatisticsController(fakeSessionService, fakeLandingPageStatisticsRepository);
        }
    }
}