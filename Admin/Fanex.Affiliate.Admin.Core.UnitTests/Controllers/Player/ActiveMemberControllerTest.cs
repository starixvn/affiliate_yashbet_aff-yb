using Fanex.Affiliate.Admin.Core.Controllers.Player;
using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Member;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Player
{
    public class ActiveMemberControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateActiveMemberController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/ActivePlayer");

            controller.Url = urlHelperMock.Object;
            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Player/ActiveMember.cshtml", viewName);
        }

        [Fact]
        public void GetAllMembers_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var userNames = "abc";
            string passedParam = null;
            var activeMemberService = new Mock<IActiveMemberService>();
            activeMemberService.Setup(t => t.GetAllMembers(userNames)).Returns(new AllMemberEntities
            {
                ActivedMemeberList = new List<ActiveMemberInfo> { new ActiveMemberInfo() },
                InactiveMemberList = new List<InactiveMemberInfo> { new InactiveMemberInfo() },
                MemberNotExist = ""
            }).Callback<string>(p => passedParam = p);
            var controller = CreateActiveMemberController(activeMemberService: activeMemberService.Object);

            // Action
            var result = controller.GetAllMembers(userNames) as JsonNetResult;
            var allMemberEntities = result.Data as AllMemberEntities;

            // Assert
            Assert.Single(allMemberEntities.ActivedMemeberList);
            Assert.Single(allMemberEntities.InactiveMemberList);
            Assert.Equal("", allMemberEntities.MemberNotExist);
            Assert.Equal(userNames, passedParam);
        }


        //[Fact]
        //public void ActiveMember_WhenCallWithValidParam_ActiveSuccessful()
        //{
        //    // Arrange
        //    var param = new ActiveMemberParam();
        //    ActiveMemberParam passedParam = null;
        //    var activeMemberService = new Mock<IActiveMemberService>();
        //    activeMemberService.Setup(s => s.ActiveMember(param)).Returns(new ResultInfo()).Callback<ActiveMemberParam>(p => passedParam = p);
        //    var controller = CreateActiveMemberController(activeMemberService: activeMemberService.Object);

        //    // Action
        //    var result = controller.ActiveMember(param) as JsonNetResult;
        //    var resultInfo = result.Data as ResultInfo;

        //    // Assert
        //    Assert.True(resultInfo.IsSuccessful);
        //    Assert.Equal(param, passedParam);
        //}

        //[Fact]
        //public void ActiveMember_WhenCallWithInvalidParam_ReturnError()
        //{
        //    // Arrange
        //    var controller = CreateActiveMemberController();
        //    controller.ModelState.AddModelError("Error01", string.Empty);

        //    // Action
        //    var result = controller.ActiveMember(new ActiveMemberParam()) as JsonNetResult;
        //    var resultInfo = result.Data as ResultInfo;

        //    // Assert
        //    Assert.False(resultInfo.IsSuccessful);
        //    Assert.Equal("Deposit User Name(s) is required!", resultInfo.Message);
        //}

        private ActiveMemberController CreateActiveMemberController(
        ISessionService sessionService = null,
        IActiveMemberService activeMemberService = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeActiveMemberService = activeMemberService ?? Mock.Of<IActiveMemberService>();

            var controller = new ActiveMemberController(fakeSessionService, fakeActiveMemberService);

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
