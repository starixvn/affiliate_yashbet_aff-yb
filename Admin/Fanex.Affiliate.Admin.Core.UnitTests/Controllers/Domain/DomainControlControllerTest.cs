using Fanex.Affiliate.Admin.Core.Controllers.Domain;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Domain;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.ViewModels.Domain;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Domain
{
    public class DomainControlControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var languageRepository = new Mock<ILanguageRepository>();
            languageRepository.Setup(l => l.GetLanguageList()).Returns(new List<Language> { new Language() });
            var controller = CreateDomainControlController(languageRepository: languageRepository.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal/DomainControl");

            controller.Url = urlHelperMock.Object;

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as DomainControlViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Domain/DomainControl.cshtml", viewName);
            Assert.Single(model.MarketList);
        }

        [Fact]
        public void GetDomainControlList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var market = "en";
            string passedParam = null;
            var domainControlService = new Mock<IDomainControlService>();
            domainControlService.Setup(t => t.GetDomainControlList(It.IsAny<string>())).Returns(new List<DomainControlInfo> { new DomainControlInfo() }).Callback<string>(p => passedParam = p);
            var controller = CreateDomainControlController(domainControlService: domainControlService.Object);

            // Action
            var result = controller.GetDomainControlList(market) as JsonNetResult;
            var data = result.Data as List<DomainControlInfo>;

            // Assert
            Assert.Single(data);
            Assert.Equal(market, passedParam);
        }

        [Fact]
        public void InsertDomainControl_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDomainControlParam { AffDomainId = 1, RedirectDomainId = 2 };
            InsertUpdateDomainControlParam passedParam = null;
            var domainControlService = new Mock<IDomainControlService>();
            domainControlService.Setup(s => s.InsertDomainControl(It.IsAny<InsertUpdateDomainControlParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainControlParam>(p => passedParam = p);
            var controller = CreateDomainControlController(domainControlService: domainControlService.Object);

            // Action
            var result = controller.InsertDomainControl(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.AffDomainId, passedParam.AffDomainId);
        }

        [Fact]
        public void InsertDomainControl_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateDomainControlController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertDomainControl(new InsertUpdateDomainControlParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Domain's info is invalid!", resultInfo.Message);
        }

        [Fact]
        public void UpdateDomainControl_WhenCallWithValidParam_InsertSuccessful()
        {
            // Arrange
            var param = new InsertUpdateDomainControlParam { DomainId = 123, AffDomainId = 1, RedirectDomainId = 2 };
            InsertUpdateDomainControlParam passedParam = null;
            var domainControlService = new Mock<IDomainControlService>();
            domainControlService.Setup(s => s.UpdateDomainControl(It.IsAny<InsertUpdateDomainControlParam>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainControlParam>(p => passedParam = p);
            var controller = CreateDomainControlController(domainControlService: domainControlService.Object);

            // Action
            var result = controller.UpdateDomainControl(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param.DomainId, passedParam.DomainId);
        }

        [Fact]
        public void UpdateDomainControl_WhenCallWithInvalidDomainId_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDomainControlParam { AffDomainId = 1, RedirectDomainId = 2 };
            var controller = CreateDomainControlController();

            // Action
            var result = controller.UpdateDomainControl(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Domain Id is invalid.", resultInfo.Message);
        }

        [Fact]
        public void UpdateDomainControl_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateDomainControlParam { DomainId = 123, RedirectDomainId = 2 };
            var controller = CreateDomainControlController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.UpdateDomainControl(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Domain's info is invalid!", resultInfo.Message);
        }

        [Fact]
        public void DeleteDomainControl_WhenCallWithValidParam_DeleteSuccessful()
        {
            // Arrange
            var param = new DeleteDomainControlParam { DomainId = 123 };
            DeleteDomainControlParam passedParam = null;
            var domainControlService = new Mock<IDomainControlService>();
            domainControlService.Setup(s => s.DeleteDomainControl(It.IsAny<DeleteDomainControlParam>())).Returns(new ResultInfo()).Callback<DeleteDomainControlParam>(p => passedParam = p);
            var controller = CreateDomainControlController(domainControlService: domainControlService.Object);

            // Action
            var result = controller.DeleteDomainControl(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void DeleteDomainControl_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateDomainControlController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.DeleteDomainControl(new DeleteDomainControlParam()) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal("Domain Id is invalid.", resultInfo.Message);
        }

        private DomainControlController CreateDomainControlController(
        ISessionService sessionService = null,
        IDomainControlService domainControlService = null,
        ILanguageRepository languageRepository = null
        )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeDomainControlService = domainControlService ?? Mock.Of<IDomainControlService>();
            var fakeLanguageRepository = languageRepository ?? Mock.Of<ILanguageRepository>();

            var controller = new DomainControlController(fakeSessionService, fakeDomainControlService, fakeLanguageRepository);

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
