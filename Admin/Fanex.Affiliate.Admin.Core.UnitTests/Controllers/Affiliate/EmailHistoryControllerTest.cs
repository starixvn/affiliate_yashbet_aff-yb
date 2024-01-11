using Fanex.Affiliate.Admin.Core.Controllers.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.EmailHistory;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.EmailHistory;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.EmailHistory.Admin.Core.UnitTests.Controllers.EmailHistory
{
    public class EmailHistoryControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateEmailHistoryController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/EmailHistory");

            controller.Url = urlHelperMock.Object;
            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/EmailHistory.cshtml", viewName);
        }

        [Fact]
        public void GetEmailHistoryList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new EmailHistoryParam();
            EmailHistoryParam passedParam = null;
            var emailHistoryService = new Mock<IEmailHistoryService>();
            emailHistoryService.Setup(t => t.GetEmailHistoryList(param)).Returns(new EntityResult<EmailHistoryInfo> { MainData = new List<EmailHistoryInfo> { new EmailHistoryInfo() }, TotalItem = 1 }).Callback<EmailHistoryParam>(p => passedParam = p);
            var controller = CreateEmailHistoryController(emailHistoryService: emailHistoryService.Object);

            // Action
            var result = controller.GetEmailHistoryList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<EmailHistoryInfo>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetEmailHistoryList_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateEmailHistoryController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetEmailHistoryList(new EmailHistoryParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetEmailHistoryDetail_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = 123;
            int passedParam = 0;
            var emailHistoryService = new Mock<IEmailHistoryService>();
            emailHistoryService.Setup(t => t.GetEmailHistoryDetail(param)).Returns(new EmailHistoryDetailInfo { EmailId = 123 }).Callback<int>(p => passedParam = p);
            var controller = CreateEmailHistoryController(emailHistoryService: emailHistoryService.Object);

            // Action
            var result = controller.GetEmailHistoryDetail(param) as JsonNetResult;
            var entityResult = result.Data as EmailHistoryDetailInfo;

            // Assert
            Assert.Equal(param, entityResult.EmailId);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetEmailHistoryDetail_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateEmailHistoryController();

            // Action
            var result = controller.GetEmailHistoryDetail(0) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void DeleteEmailHistory_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new DeleteEmailHistoryParam { EmailIdList = "123" };
            DeleteEmailHistoryParam passedParam = null;
            var emailHistoryService = new Mock<IEmailHistoryService>();
            emailHistoryService.Setup(s => s.DeleteEmailHistory(It.IsAny<DeleteEmailHistoryParam>())).Returns(new ResultInfo()).Callback<DeleteEmailHistoryParam>(p => passedParam = p);
            var controller = CreateEmailHistoryController(emailHistoryService: emailHistoryService.Object);

            // Action
            var result = controller.DeleteEmailHistory(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.EmailIdList, passedParam.EmailIdList);
        }

        [Fact]
        public void DeleteEmailHistory_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateEmailHistoryController();
            controller.ModelState.AddModelError("Error01", "Failed!");

            // Action
            var result = controller.DeleteEmailHistory(new DeleteEmailHistoryParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Failed!", resultInfo.Message);
        }

        private EmailHistoryController CreateEmailHistoryController(
            ISessionService sessionService = null,
            IEmailHistoryService emailHistoryService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeEmailHistoryService = emailHistoryService ?? Mock.Of<IEmailHistoryService>();

            var controller = new EmailHistoryController(fakeSessionService, fakeEmailHistoryService);

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