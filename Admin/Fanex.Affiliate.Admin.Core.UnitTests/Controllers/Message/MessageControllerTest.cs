using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Message;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Message;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Message;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Message
{
    public class MessageControllerTest
    {

        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateMessageController();

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void GetMessageList_WhenCallWithValidParamAndGetAllDate_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new MessageParam { IsGetAll = true };
            MessageParam passedParam = null;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.GetMessageList(param)).Returns(new EntityResult<MessageInfo> { MainData = new List<MessageInfo> { new MessageInfo() }, TotalItem = 1 }).Callback<MessageParam>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.GetMessageList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<MessageInfo>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetMessageList_WhenCallWithValidParamAndNotGetAllDate_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new MessageParam { IsGetAll = false, FromDate = new DateTime(), ToDate = new DateTime() };
            MessageParam passedParam = null;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.GetMessageList(param)).Returns(new EntityResult<MessageInfo> { MainData = new List<MessageInfo> { new MessageInfo() }, TotalItem = 1 }).Callback<MessageParam>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.GetMessageList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<MessageInfo>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetMessageList_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateMessageController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetMessageList(new MessageParam()) as JsonNetResult;
            var entityResult = result.Data as EntityResult<MessageInfo>;

            // Assert
            Assert.Empty(entityResult.MainData);
            Assert.Equal(0, entityResult.TotalItem);
        }

        [Fact]
        public void GetMessageDetail_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var selectedMessageId = 37;
            var passedParam = 0;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.GetMessageDetail(selectedMessageId)).Returns(new MessageDetail { MessageId = 37 }).Callback<int>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.GetMessageDetail(selectedMessageId) as JsonNetResult;
            var messageDetail = result.Data as MessageDetail;

            // Assert
            Assert.Equal(selectedMessageId, messageDetail.MessageId);
            Assert.Equal(selectedMessageId, passedParam);
        }

        [Fact]
        public void InsertMessage_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateMessageParam { MessageContenten = "abc" };
            InsertUpdateMessageParam passedParam = null;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.InsertMessage(It.IsAny<InsertUpdateMessageParam>())).Returns(new ResultInfo()).Callback<InsertUpdateMessageParam>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.InsertMessage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.MessageId, passedParam.MessageId);
        }

        [Fact]
        public void InsertMessage_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateMessageController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertMessage(new InsertUpdateMessageParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please enter the content of message, one language at least.", resultInfo.Message);
        }

        [Fact]
        public void UpdateMessage_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateMessageParam { MessageId = 1, MessageContentvn = "abc" };
            InsertUpdateMessageParam passedParam = null;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.UpdateMessage(It.IsAny<InsertUpdateMessageParam>())).Returns(new ResultInfo()).Callback<InsertUpdateMessageParam>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.UpdateMessage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.MessageId, passedParam.MessageId);
        }

        [Fact]
        public void UpdateMessage_WhenCallWithInvalidMessageContent_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMessageParam { MessageId = 1 };
            var controller = CreateMessageController();

            // Action
            var result = controller.UpdateMessage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please enter the content of message, one language at least.", resultInfo.Message);
        }

        [Fact]
        public void UpdateMessage_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMessageParam { MessageContenten = "abc" };
            var controller = CreateMessageController();

            // Action
            var result = controller.UpdateMessage(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("MessageId is invalid.", resultInfo.Message);
        }

        [Fact]
        public void DeleteMessages_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new DeleteMessageParam { MessageIdList = "1" };
            DeleteMessageParam passedParam = null;
            var messageSevice = new Mock<IMessageService>();
            messageSevice.Setup(s => s.DeleteMessages(param)).Returns(new ResultInfo()).Callback<DeleteMessageParam>(p => passedParam = p);
            var controller = CreateMessageController(messageSevice: messageSevice.Object);

            // Action
            var result = controller.DeleteMessages(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteMessages_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateMessageController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteMessages(new DeleteMessageParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please select messages to delete.", resultInfo.Message);
        }

        private MessageController CreateMessageController(
            ISessionService sessionService = null,
            IMessageService messageSevice = null,
            ILanguageService languageService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeMessageSevice = messageSevice ?? Mock.Of<IMessageService>();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();

            var controller = new MessageController(fakeSessionService, fakeMessageSevice, fakeLanguageService);

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
