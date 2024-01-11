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
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class ReferrersStatisticsControllerTest
    {
        [Fact]
        public void IndexGet_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            var referrerStatisticsList = new List<ReferrerStatistics> { new ReferrerStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetReferrerStatistics(It.IsAny<ReferrerStatisticsParam>())).Returns(referrerStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var today = DateTime.Today;
            var expectedViewModel = new ReferrerStatisticsViewModel
            {
                DataList = referrerStatisticsList,
                FromDate = new DateTime(today.Year, today.Month, 1),
                ToDate = today
            };

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as ReferrerStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/ReferrersStatistics/Index.cshtml", viewName);
            Assert.Equal(expectedViewModel.DataList, model.DataList);
            Assert.Equal(expectedViewModel.FromDate, model.FromDate);
            Assert.Equal(expectedViewModel.ToDate, model.ToDate.Date);
        }

        [Fact]
        public void IndexPost_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            var referrerStatisticsList = new List<ReferrerStatistics> { new ReferrerStatistics() };

            var statisticsRepository = new Mock<IStatisticsRepository>();
            statisticsRepository.Setup(s => s.GetReferrerStatistics(It.IsAny<ReferrerStatisticsParam>())).Returns(referrerStatisticsList);

            var controller = CreateController(staticticsRepository: statisticsRepository.Object);

            var expectedViewModel = new ReferrerStatisticsViewModel
            {
                DataList = referrerStatisticsList
            };

            var param = new ReferrerStatisticsParam();
            var expectedParam = new ReferrerStatisticsParam
            {
                AffId = Constant.DefaultAffId,
                IsInternal = false
            };

            // Action
            var viewResult = controller.Index(param) as PartialViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as ReferrerStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/StatisticsPartial/_ReferrersStatisticsContent.cshtml", viewName);
            Assert.Equal(expectedViewModel.DataList, model.DataList);
            Assert.Equal(expectedParam.AffId, param.AffId);
            Assert.Equal(expectedParam.IsInternal, param.IsInternal);
        }

        private static ReferrersStatisticsController CreateController(
            ISessionService sessionService = null,
            ILanguageService languageService = null,
            IStatisticsRepository staticticsRepository = null)
        {
            var appSetting = Mock.Of<IAppSetting>();
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeStaticticsRepository = staticticsRepository ?? (new Mock<IStatisticsRepository>()).Object;

            return new ReferrersStatisticsController(appSetting, fakeSessionService, fakeLanguageService, fakeStaticticsRepository);
        }
    }
}
