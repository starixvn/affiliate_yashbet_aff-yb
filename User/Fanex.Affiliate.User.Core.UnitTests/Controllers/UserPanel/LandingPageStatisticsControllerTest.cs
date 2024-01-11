using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class LandingPageStatisticsControllerTest
    {
        private const bool IsInternal = false;

        [Fact]
        public void IndexGet_WhenCall_ReturnViewWithCorrectModelData()
        {
            // Arrange
            var marketingService = new Mock<IMarketingService>();
            marketingService.Setup(m => m.GetLandingPageList(IsInternal, It.IsAny<AffiliateLinkType>())).Returns(new List<LandingPage>());
            var languageList = CreateFakeLanguageList();
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.GetLanguageList(IsInternal)).Returns(languageList);

            var statisticsRepository = new Mock<IStatisticsRepository>();
            var landingPageStisticsList = CreateFakeLandingPageStatisticsList();
            statisticsRepository.Setup(m => m.GetLandingPageStatistics(It.IsAny<LandingPageStatisticsParam>())).Returns(landingPageStisticsList);

            var today = DateTime.Today;
            var controller = CreateController(null, languageService.Object, statisticsRepository.Object, marketingService.Object);

            // Action
            var result = controller.Index() as ViewResult;
            var model = result.Model as LandingPageStatisticsViewModel;

            // Assert
            Assert.Equal(2, model.LanguageList.Count());
            Assert.Equal(2, model.DataList.Count());
            Assert.False(model.IsDetail);
            Assert.Equal(new DateTime(today.Year, today.Month, 1), model.FromDate.Date);
            Assert.Equal(today, model.ToDate.Date);
            Assert.Equal(0, model.RowNumber);
        }

        [Fact]
        public void IndexGet_WhenCall_CallGetLandingPageStatisticsWithCorrectParam()
        {
            // Arrange
            LandingPageStatisticsParam passedParam = null;
            var marketingService = new Mock<IMarketingService>();
            marketingService.Setup(m => m.GetLandingPageList(IsInternal, It.IsAny<AffiliateLinkType>())).Returns(new List<LandingPage>());
            var languageList = CreateFakeLanguageList();
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.GetLanguageList(IsInternal)).Returns(languageList);

            var statisticsRepository = new Mock<IStatisticsRepository>();
            var landingPageStisticsList = CreateFakeLandingPageStatisticsList();
            statisticsRepository.Setup(m => m.GetLandingPageStatistics(It.IsAny<LandingPageStatisticsParam>()))
                .Returns(landingPageStisticsList)
                .Callback<LandingPageStatisticsParam>(p => passedParam = p);

            var today = DateTime.Today;
            var controller = CreateController(null, languageService.Object, statisticsRepository.Object, marketingService.Object);

            // Action
            controller.Index();

            // Assert
            Assert.Equal(Constant.DefaultAffId, passedParam.AffId);
            Assert.Equal(IsInternal, passedParam.IsInternal);
            Assert.Equal(new DateTime(today.Year, today.Month, 1), passedParam.FromDate.Date);
            Assert.Equal(today, passedParam.ToDate.Date);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("en-US", "en")]
        [InlineData("th-TH", null)]
        public void IndexPost_WhenCall_ReturnViewWithCorrectModelData(string languageCode, string expectedShortedCode)
        {
            // Arrange
            var param = new LandingPageStatisticsParam
            {
                LanguageCode = languageCode
            };
            var statisticsRepository = new Mock<IStatisticsRepository>();
            var landingPageStisticsList = CreateFakeLandingPageStatisticsList();
            statisticsRepository.Setup(m => m.GetLandingPageStatistics(param)).Returns(landingPageStisticsList);

            var languageService = new Mock<ILanguageService>();
            languageService.Setup(m => m.GetLanguageList(It.IsAny<bool?>())).Returns(Constant.LanguageList);
            var controller = CreateController(null, languageService.Object, statisticsRepository.Object);

            // Action
            var result = controller.Index(param) as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model as LandingPageStatisticsViewModel;

            // Assert
            Assert.Equal("~/Views/UserPanel/Statistics/StatisticsPartial/_LandingPageStatisticsContent.cshtml", viewName);
            Assert.Equal(2, model.DataList.Count());
            Assert.False(model.IsDetail);
            Assert.Equal(Constant.DefaultAffId, param.AffId);
            Assert.Equal(IsInternal, param.IsInternal);
            Assert.Equal(expectedShortedCode, param.LanguageCode);
        }

        private IEnumerable<Language> CreateFakeLanguageList()
        {
            return new List<Language>
            {
                new Language
                {
                    Code = "en-US",
                    DisplayName = "English",
                },
                new Language
                {
                    Code = "vi-VN",
                    DisplayName = "Vietnamese",
                }
            };
        }

        private IEnumerable<LandingPageStatistics> CreateFakeLandingPageStatisticsList()
        {
            return new List<LandingPageStatistics>
            {
                new LandingPageStatistics { LanguageCode = "abc" },
                new LandingPageStatistics { LanguageCode = "en" }
            };
        }

        private static LandingPageStatisticsController CreateController(
            ISessionService sessionService = null,
            ILanguageService languageService = null,
            IStatisticsRepository staticticsRepository = null,
            IMarketingService marketingService = null)
        {
            var appSetting = Mock.Of<IAppSetting>();
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeStaticticsRepository = staticticsRepository ?? Mock.Of<IStatisticsRepository>();
            var fakeMarketingService = marketingService ?? Mock.Of<IMarketingService>();

            return new LandingPageStatisticsController(
                appSetting,
                fakeSessionService,
                fakeLanguageService,
                fakeStaticticsRepository,
                fakeMarketingService);
        }
    }
}
