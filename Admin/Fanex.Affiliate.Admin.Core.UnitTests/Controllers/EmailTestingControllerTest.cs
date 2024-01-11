using Autofac;
using Autofac.Integration.Mvc;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.Utilities;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers
{
    public class EmailTestingControllerTest
    {
        private ILifetimeScopeProvider scopeProvider;

        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateEmailTestingController();

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallHasException_ReturnView()
        {
            // Arrange
            var controller = CreateEmailTestingController(MockingHelper.GetSessionService());

            // Action
            var viewResult = controller.Preview(string.Empty, string.Empty, string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallWithAffiliateDisapproved_ReturnView()
        {
            // Arrange
            var languageService = new Mock<ILanguageService>();
            languageService.Setup(l => l.GetLanguageList()).Returns(new List<Language> { new Language { LanguageCode = "en-US", ShortedLanguageCode = "en" } });

            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(null, null, appSetting.Object, languageService.Object);

            // Action
            var viewResult = controller.Preview("en-US", "AffiliateDisapproved", string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallWithAffiliateInformation_ReturnView()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.DefaultShortedLanguageCode).Returns("en");
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(appSetting: appSetting.Object);

            // Action
            var viewResult = controller.Preview(string.Empty, "AffiliateInformation", string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallWithWithdrawalGuidance_ReturnView()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.DefaultShortedLanguageCode).Returns("en");
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(appSetting: appSetting.Object);

            // Action
            var viewResult = controller.Preview(string.Empty, "WithdrawalGuidance", string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallPaymentRequestApproved_ReturnView()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.DefaultShortedLanguageCode).Returns("en");
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(appSetting: appSetting.Object);

            // Action
            var viewResult = controller.Preview(string.Empty, "PaymentRequest/Approved", string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void Preview_WhenCallPaymentRequestRejected_ReturnView()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.DefaultShortedLanguageCode).Returns("en");
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(appSetting: appSetting.Object);

            // Action
            var viewResult = controller.Preview(string.Empty, "PaymentRequest/Rejected", string.Empty) as ViewResult;
            var viewName = viewResult.ViewName;

            // Assert
            Assert.Equal("", viewName);
        }

        [Fact]
        public void SendEmail_WhenCallHasException_ReturnFalse()
        {
            // Arrange
            var controller = CreateEmailTestingController(MockingHelper.GetSessionService());

            // Action
            var jsonResult = controller.SendEmail(string.Empty, string.Empty, string.Empty, string.Empty) as JsonNetResult;
            var data = jsonResult.Data as dynamic;

            // Assert
            Assert.False(data.Success);
        }

        [Theory]
        [InlineData("AffiliateDisapproved")]
        [InlineData("AffiliateInformation")]
        [InlineData("PaymentRequest/Approved")]
        [InlineData("PaymentRequest/Rejected")]
        [InlineData("")]
        public void SendEmail_WhenCall_ReturnTrue(string function)
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);
            appSetting.Setup(a => a.DefaultShortedLanguageCode).Returns("en");
            appSetting.Setup(a => a.GetMailContactByShortedLanguageCode(It.IsAny<string>())).Returns("abc@gmail.com");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var controller = CreateEmailTestingController(appSetting: appSetting.Object);

            // Action
            var jsonResult = controller.SendEmail(string.Empty, function, string.Empty, string.Empty) as JsonNetResult;
            var data = jsonResult.Data as dynamic;

            // Assert
            Assert.True(data.Success);
        }

        private EmailTestingController CreateEmailTestingController(
            ISessionService sessionService = null,
            IMailProcessor mailProcessor = null,
            IAppSetting appSetting = null,
            ILanguageService languageService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeMailProcessor = mailProcessor ?? Mock.Of<IMailProcessor>();
            var fakeAppSetting = appSetting ?? Mock.Of<IAppSetting>();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();

            var controller = new EmailTestingController(fakeSessionService, fakeMailProcessor, fakeAppSetting, fakeLanguageService);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            var expectedPath = "~/";
            var expectedContent = "/";
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock
                .Setup(m => m.Content(expectedPath))
                .Returns(expectedContent);

            controller.Url = urlHelperMock.Object;

            return controller;
        }
    }
}