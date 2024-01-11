using Fanex.Affiliate.Admin.Core.Controllers.MediaGallery;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.MediaGallery
{
    public class LandingPageControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateLandingPageListController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/LandingPageList");

            controller.Url = urlHelperMock.Object;
            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/MediaGallery/LandingPageList.cshtml", viewName);
        }

        [Fact]
        public void GetLandingPageList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var landingPageService = new Mock<ILandingPageService>();
            landingPageService.Setup(t => t.GetLandingPageList()).Returns(new List<LandingPage> { new LandingPage() });
            var controller = CreateLandingPageListController(landingPageService: landingPageService.Object);

            // Action
            var result = controller.GetLandingPageList() as JsonNetResult;
            var data = result.Data as List<LandingPage>;

            // Assert
            Assert.Single(data);
        }

        [Fact]
        public void GetLandingPageItem_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var pageId = 22;
            int passedParam = 0;
            var landingPageService = new Mock<ILandingPageService>();
            landingPageService.Setup(t => t.GetLandingPageById(pageId)).Returns(new LandingPage { PageId = 22 }).Callback<int>(p => passedParam = p);
            var controller = CreateLandingPageListController(landingPageService: landingPageService.Object);

            // Action
            var result = controller.GetLandingPageItem(pageId) as JsonNetResult;
            var data = result.Data as LandingPage;

            // Assert
            Assert.Equal(pageId, data.PageId);
            Assert.Equal(pageId, passedParam);
        }

        [Fact]
        public void InsertLandingPage_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageName = "123", PageCode = "abc" };
            InsertUpdateLandingPageParam passedParam = null;
            var landingPageService = new Mock<ILandingPageService>();
            landingPageService.Setup(s => s.InsertLandingPage(It.IsAny<InsertUpdateLandingPageParam>())).Returns(new ResultInfo()).Callback<InsertUpdateLandingPageParam>(p => passedParam = p);
            var controller = CreateLandingPageListController(landingPageService: landingPageService.Object);

            // Action
            var result = controller.InsertLandingPage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.PageId, passedParam.PageId);
        }

        [Fact]
        public void InsertLandingPage_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateLandingPageListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertLandingPage(new InsertUpdateLandingPageParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(string.Empty, resultInfo.Message);
        }

        [Fact]
        public void UpdateLandingPage_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageId = 123, PageName = "123", PageCode = "abc" };
            InsertUpdateLandingPageParam passedParam = null;
            var landingPageService = new Mock<ILandingPageService>();
            landingPageService.Setup(s => s.UpdateLandingPage(It.IsAny<InsertUpdateLandingPageParam>())).Returns(new ResultInfo()).Callback<InsertUpdateLandingPageParam>(p => passedParam = p);
            var controller = CreateLandingPageListController(landingPageService: landingPageService.Object);

            // Action
            var result = controller.UpdateLandingPage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.PageId, passedParam.PageId);
        }

        [Fact]
        public void UpdateText_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageId = 123 };
            var controller = CreateLandingPageListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateLandingPage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(string.Empty, resultInfo.Message);
        }

        [Fact]
        public void DeleteLandingPage_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new DeleteLandingPageParam { PageIds = "123" };
            DeleteLandingPageParam passedParam = null;
            var landingPageService = new Mock<ILandingPageService>();
            landingPageService.Setup(s => s.DeleteLandingPage(param)).Returns(new ResultInfo()).Callback<DeleteLandingPageParam>(p => passedParam = p);
            var controller = CreateLandingPageListController(landingPageService: landingPageService.Object);

            // Action
            var result = controller.DeleteLandingPage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteLandingPage_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateLandingPageListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteLandingPage(new DeleteLandingPageParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(string.Empty, resultInfo.Message);
        }

        private LandingPageListController CreateLandingPageListController(
        ISessionService sessionService = null,
        ILandingPageService landingPageService = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLandingPageService = landingPageService ?? Mock.Of<ILandingPageService>();

            var controller = new LandingPageListController(fakeSessionService, fakeLandingPageService);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            return controller;
        }
    }
}
