using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class NewsControllerTest
    {
        [Fact]
        public void MessageHistory_WhenCall_ReturnCorrectViewModel()
        {
            // Arrange
            const int TotalItem = 10;
            var messageList = new List<MessageHistory> { new MessageHistory() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetMessageList(It.IsAny<MessageHistoryParam>()))
                        .Returns(messageList)
                        .Callback<MessageHistoryParam>(p => p.Nrows = TotalItem);

            var controller = CreateController(newsService.Object);

            var expectedResult = JsonConvert.SerializeObject(new MessageHistoryViewModel
            {
                MessageList = messageList,
                TotalItems = TotalItem
            });

            // Action
            var actionResult = controller.MessageHistory() as ViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MessageHistory_WhenCall_ReturnCorrectViewName()
        {
            // Arrange
            const int TotalItem = 10;
            var messageList = new List<MessageHistory> { new MessageHistory() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetMessageList(It.IsAny<MessageHistoryParam>()))
                        .Returns(messageList)
                        .Callback<MessageHistoryParam>(p => p.Nrows = TotalItem);

            var controller = CreateController(newsService.Object);

            // Action
            var actionResult = controller.MessageHistory() as ViewResult;
            var result = actionResult.ViewName;

            // Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MessageHistory_CallService_PassParamCorrectly(bool isInternal)
        {
            // Arrange
            MessageHistoryParam passedParam = null;
            var messageList = new List<MessageHistory> { new MessageHistory() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetMessageList(It.IsAny<MessageHistoryParam>()))
                        .Returns(messageList)
                        .Callback<MessageHistoryParam>(p => passedParam = p);

            var sessionService = MockingHelper.GetSessionService();
            sessionService.IsInternal = isInternal;

            var controller = CreateController(newsService.Object, sessionService);

            var expectedResult = JsonConvert.SerializeObject(new MessageHistoryParam
            {
                IsInternal = isInternal,
                Important = null,
                Language = Constant.DefaultLanguage
            });

            // Action
            controller.MessageHistory();
            var result = JsonConvert.SerializeObject(passedParam);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(false, true)]
        public void GetLatestHotNew_CheckLoggedInAndIsInternal_ReturnModelCorrectly(bool isLoggedIn, bool isInternal)
        {
            // Arrange
            var sessionService = Mock.Of<ISessionService>(s =>
                s.IsInternal == isInternal &&
                s.UserSession == (isLoggedIn ? new UserSession() : null));

            var hotNews = new LatestHotNews { HotNewsList = new Dictionary<string, string>() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetLatestHotNews(isLoggedIn && isInternal)).Returns(hotNews);

            var principal = Mock.Of<IPrincipal>(p => p.Identity.IsAuthenticated == isLoggedIn);
            var controllerContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == principal);

            var controller = CreateController(newsService.Object, sessionService);
            controller.ControllerContext = controllerContext;

            var expectedResult = JsonConvert.SerializeObject(new LatestHotNewViewModel
            {
                IsAuthenticated = isLoggedIn,
                IsImportant = false
            });

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
            newsService.Verify(ns => ns.GetLatestHotNews(isLoggedIn && isInternal), Times.Once);
        }

        [Fact]
        public void GetLatestHotNew_WhenCall_ReturnViewNameCorrectly()
        {
            // Arrange
            var sessionService = Mock.Of<ISessionService>();

            var hotNews = new LatestHotNews { HotNewsList = new Dictionary<string, string>() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetLatestHotNews(false)).Returns(hotNews);

            var principal = Mock.Of<IPrincipal>(p => p.Identity.IsAuthenticated);
            var controllerContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == principal);

            var controller = CreateController(newsService.Object, sessionService);
            controller.ControllerContext = controllerContext;

            const string ExpectedResult = "~/Views/News/_LatestHotNews.cshtml";

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = actionResult.ViewName;

            // Assert
            Assert.Equal(ExpectedResult, result);
        }

        [Fact]
        public void GetLatestHotNew_HasHotNewsDataAndContainPreferredLanguage_GetMessageCorrectly()
        {
            // Arrange
            const bool IsInternal = false;
            var hotNews = new LatestHotNews
            {
                IsImportant = true,
                HotNewsList = new Dictionary<string, string> { [Constant.DefaultLanguage] = $"Message {Constant.DefaultLanguage}" }
            };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetLatestHotNews(IsInternal)).Returns(hotNews);

            var principal = Mock.Of<IPrincipal>(p => p.Identity.IsAuthenticated);
            var controllerContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == principal);

            var controller = CreateController(newsService.Object);
            controller.ControllerContext = controllerContext;

            var expectedResult = JsonConvert.SerializeObject(new LatestHotNewViewModel
            {
                IsAuthenticated = true,
                IsImportant = true,
                SelectedHotNews = hotNews.HotNewsList[Constant.DefaultLanguage]
            });

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLatestHotNew_HasHotNewsDataButNotContainPreferredLanguage_GetFirstMessage()
        {
            // Arrange
            const string OtherLanguageCode = "Other";
            const bool IsInternal = false;
            var hotNews = new LatestHotNews
            {
                IsImportant = true,
                HotNewsList = new Dictionary<string, string> { [OtherLanguageCode] = $"Message {OtherLanguageCode}" }
            };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetLatestHotNews(IsInternal)).Returns(hotNews);

            var principal = Mock.Of<IPrincipal>(p => p.Identity.IsAuthenticated);
            var controllerContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == principal);

            var controller = CreateController(newsService.Object);
            controller.ControllerContext = controllerContext;

            var expectedResult = JsonConvert.SerializeObject(new LatestHotNewViewModel
            {
                IsAuthenticated = true,
                IsImportant = true,
                SelectedHotNews = hotNews.HotNewsList.First().Value
            });

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLatestHotNew_HasHotNewsDataButHotNewsListIsEmpty_ReturnCorrectView()
        {
            // Arrange
            const bool IsInternal = false;
            var hotNews = new LatestHotNews
            {
                IsImportant = true
            };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetLatestHotNews(IsInternal)).Returns(hotNews);

            var principal = Mock.Of<IPrincipal>(p => p.Identity.IsAuthenticated);
            var controllerContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == principal);

            var controller = CreateController(newsService.Object);
            controller.ControllerContext = controllerContext;

            var expectedResult = JsonConvert.SerializeObject(new LatestHotNewViewModel
            {
                IsAuthenticated = true,
                IsImportant = true
            });

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLatestHotNew_NotHasHotNews_ReturnCorrectView()
        {
            // Arrange
            var newsService = new Mock<INewsService>();
            var controller = CreateController(newsService.Object);

            var expectedResult = JsonConvert.SerializeObject(new LatestHotNewViewModel());

            // Action
            var actionResult = controller.LatestHotNews() as PartialViewResult;
            var result = JsonConvert.SerializeObject(actionResult.Model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMessageHistory_WhenCall_PassParamCorrectly(bool isInternal)
        {
            // Arrange
            var param = new MessageHistoryParam
            {
                FromDate = new DateTime(2018, 6, 12),
                ToDate = new DateTime(2018, 6, 14),
                Important = true,
                PageSize = 20,
                PageIndex = 1
            };

            MessageHistoryParam passedParam = null;

            var messageList = new List<MessageHistory> { new MessageHistory() };
            var newsService = new Mock<INewsService>();
            newsService.Setup(ns => ns.GetMessageList(param))
                        .Returns(messageList)
                        .Callback<MessageHistoryParam>(p => passedParam = p);

            var sessionService = MockingHelper.GetSessionService();
            sessionService.IsInternal = isInternal;

            var controller = CreateController(newsService.Object, sessionService);

            // Action
            controller.GetMessageHistory(param);

            // Assert
            Assert.Equal(passedParam, param);
            Assert.Equal(passedParam.IsInternal, isInternal);
            Assert.Equal(Constant.DefaultLanguage, passedParam.Language);
        }

        private static NewsController CreateController(INewsService newsService, ISessionService sessionService = null, ILanguageService languageService = null)
        {
            sessionService = sessionService ?? MockingHelper.GetSessionService();
            languageService = languageService ?? new Mock<ILanguageService>().Object;

            return new NewsController(new Mock<IAppSetting>().Object, sessionService, languageService, newsService);
        }
    }
}