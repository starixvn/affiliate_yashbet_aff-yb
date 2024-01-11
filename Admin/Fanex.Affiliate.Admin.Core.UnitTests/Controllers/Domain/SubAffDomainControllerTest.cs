using Fanex.Affiliate.Admin.Core.Controllers.Domain;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Domain;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Domain
{
    public class SubAffDomainControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateSubAffDomainController();

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/SubAffDomain");

            controller.Url = urlHelperMock.Object;

            // Action
            var viewResult = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Domain/SubAffDomain.cshtml", viewResult.ViewName);
        }

        [Fact]
        public void GetSubAffDomainList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var subAffDomainService = new Mock<ISubAffDomainService>();
            subAffDomainService.Setup(t => t.GetSubAffDomainList()).Returns(new List<DomainInfo> { new DomainInfo() });
            var controller = CreateSubAffDomainController(subAffDomainService: subAffDomainService.Object);

            // Action
            var result = controller.GetSubAffDomainList() as JsonNetResult;
            var data = result.Data as List<DomainInfo>;

            // Assert
            Assert.Single(data);
        }

        [Fact]
        public void InsertSubAffDomain_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDomainParam { DomainName = "abc.com" };
            InsertUpdateDomainParam passedParam = null;
            var subAffDomainService = new Mock<ISubAffDomainService>();
            subAffDomainService.Setup(s => s.InsertSubAffDomain(It.IsAny<InsertUpdateDomainParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam>(p => passedParam = p);
            var controller = CreateSubAffDomainController(subAffDomainService: subAffDomainService.Object);

            // Action
            var result = controller.InsertSubAffDomain(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.DomainId, passedParam.DomainId);
        }

        [Fact]
        public void InsertSubAffDomain_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateSubAffDomainController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertSubAffDomain(new InsertUpdateDomainParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input Sub-Affiliate Domain's name.", resultInfo.Message);
        }

        [Fact]
        public void UpdateSubAffDomain_WhenCallWithValidParam_UpdateSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDomainParam { DomainId = 123, DomainName = "abc.com" };
            InsertUpdateDomainParam passedParam = null;
            var subAffDomainService = new Mock<ISubAffDomainService>();
            subAffDomainService.Setup(s => s.UpdateSubAffDomain(It.IsAny<InsertUpdateDomainParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam>(p => passedParam = p);
            var controller = CreateSubAffDomainController(subAffDomainService: subAffDomainService.Object);

            // Action
            var result = controller.UpdateSubAffDomain(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.DomainId, passedParam.DomainId);
        }

        [Fact]
        public void UpdateSubAffDomain_WhenCallWithInvalidDomainId_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDomainParam { DomainName = "abc" };
            var controller = CreateSubAffDomainController();

            // Action
            var result = controller.UpdateSubAffDomain(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Sub-Affiliate Domain Id is invalid.", resultInfo.Message);
        }

        [Fact]
        public void UpdateSubAffDomain_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDomainParam { DomainId = 123, DomainName = "abc.com" };
            var controller = CreateSubAffDomainController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateSubAffDomain(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Please input Sub-Affiliate Domain's name.", resultInfo.Message);
        }

        [Fact]
        public void DeleteSubAffDomain_WhenCallWithValidParam_DeleteSuccessful()
        {
            // Arrange
            var param = new DeleteDomainParam { DomainId = 123 };
            DeleteDomainParam passedParam = null;
            var subAffDomainService = new Mock<ISubAffDomainService>();
            subAffDomainService.Setup(s => s.DeleteSubAffDomain(It.IsAny<DeleteDomainParam>())).Returns(new ResultInfo()).Callback<DeleteDomainParam>(p => passedParam = p);
            var controller = CreateSubAffDomainController(subAffDomainService: subAffDomainService.Object);

            // Action
            var result = controller.DeleteSubAffDomain(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteSubAffDomain_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateSubAffDomainController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteSubAffDomain(new DeleteDomainParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Sub-Affiliate Domain Id is incorrect.", resultInfo.Message);
        }

        private SubAffDomainController CreateSubAffDomainController(
        ISessionService sessionService = null,
        ISubAffDomainService subAffDomainService = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeSubAffDomainService = subAffDomainService ?? Mock.Of<ISubAffDomainService>();

            var controller = new SubAffDomainController(fakeSessionService, fakeSubAffDomainService);

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
