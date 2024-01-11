using Fanex.Affiliate.Admin.Core.Controllers.MediaGallery;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.ViewModels.MediaGallery;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.MediaGallery
{
    public class TextListControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var commonMediaGalleryService = new Mock<ICommonMediaGalleryService>();
            commonMediaGalleryService.Setup(c => c.GetCategoryList(2, 1)).Returns(new List<Category> { new Category { CategoryName = "Category" } });
            commonMediaGalleryService.Setup(c => c.GetCurrencyList()).Returns(new List<MediaGalleryFilterItem> { new MediaGalleryFilterItem { Id = 234, Name = "Currency" } });
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(l => l.GetLanguageList()).Returns(new List<Language> { new Language { LanguageCode = "en", LanguageName = "English" } });

            var controller = CreateTextListController(commonMediaGalleryService: commonMediaGalleryService.Object, languageService: languageService.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/TextList");

            controller.Url = urlHelperMock.Object;
            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as TextListViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/MediaGallery/TextList.cshtml", viewName);
            Assert.Single(model.LanguageList);
            Assert.Single(model.CurrencyList);
        }

        [Fact]
        public void GetCategoryList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var commonMediaGalleryService = new Mock<ICommonMediaGalleryService>();
            commonMediaGalleryService.Setup(c => c.GetCategoryList(2, 1)).Returns(new List<Category> { new Category { CategoryName = "Category" } });

            var controller = CreateTextListController(commonMediaGalleryService: commonMediaGalleryService.Object);

            // Action
            var result = controller.GetCategoryList(1) as JsonNetResult;

            // Assert
            Assert.Single(result.Data as IEnumerable<Category>);
        }

        [Fact]
        public void GetTextList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new TextParam();
            TextParam passedParam = null;
            var textListService = new Mock<ITextListService>();
            textListService.Setup(t => t.GetTextList(param)).Returns(new EntityResult<TextInfo> { MainData = new List<TextInfo> { new TextInfo() }, TotalItem = 1 }).Callback<TextParam>(p => passedParam = p);
            var controller = CreateTextListController(textListService: textListService.Object);

            // Action
            var result = controller.GetTextList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<TextInfo>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetTextList_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateTextListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetTextList(new TextParam()) as JsonNetResult;
            var entityResult = result.Data as EntityResult<TextInfo>;

            // Assert
            Assert.Empty(entityResult.MainData);
            Assert.Equal(0, entityResult.TotalItem);
        }

        [Fact]
        public void GetTextDetail_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var selectedTextId = 123;
            var textListService = new Mock<ITextListService>();
            textListService.Setup(s => s.GetTextDetail(selectedTextId, 1)).Returns(new TextInfo { TextId = 123 });
            var controller = CreateTextListController(textListService: textListService.Object);

            // Action
            var result = controller.GetTextDetail(selectedTextId, 1) as JsonNetResult;
            var textInfo = result.Data as TextInfo;

            // Assert
            Assert.Equal(selectedTextId, textInfo.TextId);
        }

        [Fact]
        public void InsertText_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateTextParam { TextContent = "abc" };
            InsertUpdateTextParam passedParam = null;
            var textListService = new Mock<ITextListService>();
            textListService.Setup(s => s.InsertText(It.IsAny<InsertUpdateTextParam>())).Returns(new ResultInfo()).Callback<InsertUpdateTextParam>(p => passedParam = p);
            var controller = CreateTextListController(textListService: textListService.Object);

            // Action
            var result = controller.InsertText(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.TextId, passedParam.TextId);
        }

        [Fact]
        public void InsertText_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateTextListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertText(new InsertUpdateTextParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input text to upload.", resultInfo.Message);
        }

        [Fact]
        public void UpdateText_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new InsertUpdateTextParam { TextId = 123, TextContent = "abc" };
            InsertUpdateTextParam passedParam = null;
            var textListService = new Mock<ITextListService>();
            textListService.Setup(s => s.UpdateText(It.IsAny<InsertUpdateTextParam>())).Returns(new ResultInfo()).Callback<InsertUpdateTextParam>(p => passedParam = p);
            var controller = CreateTextListController(textListService: textListService.Object);

            // Action
            var result = controller.UpdateText(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.TextId, passedParam.TextId);
        }

        [Fact]
        public void UpdateText_WhenCallWithInvalidTextId_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateTextParam { TextContent = "abc" };
            var controller = CreateTextListController();

            // Action
            var result = controller.UpdateText(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("TextId is invalid.", resultInfo.Message);
        }

        [Fact]
        public void UpdateText_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateTextParam { TextId = 123 };
            var controller = CreateTextListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateText(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input text to update.", resultInfo.Message);
        }

        [Fact]
        public void DeleteTexts_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new DeleteTextParam { TextIdList = "123" };
            DeleteTextParam passedParam = null;
            var textListService = new Mock<ITextListService>();
            textListService.Setup(s => s.DeleteTexts(param)).Returns(new ResultInfo()).Callback<DeleteTextParam>(p => passedParam = p);
            var controller = CreateTextListController(textListService: textListService.Object);

            // Action
            var result = controller.DeleteTexts(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteTexts_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateTextListController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteTexts(new DeleteTextParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please select texts to delete.", resultInfo.Message);
        }

        private TextListController CreateTextListController(
            ISessionService sessionService = null,
            ILanguageService languageService = null,
            ICommonMediaGalleryService commonMediaGalleryService = null,
            ITextListService textListService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeCommonMediaGalleryService = commonMediaGalleryService ?? Mock.Of<ICommonMediaGalleryService>();
            var fakeTextListService = textListService ?? Mock.Of<ITextListService>();

            var controller = new TextListController(fakeSessionService, fakeLanguageService, fakeCommonMediaGalleryService, fakeTextListService);

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
