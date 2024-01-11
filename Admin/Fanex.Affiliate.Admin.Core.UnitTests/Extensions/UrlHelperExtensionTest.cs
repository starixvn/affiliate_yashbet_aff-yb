using Autofac;
using Autofac.Integration.Mvc;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Extensions
{
    public class UrlHelperExtensionTest
    {
        private ILifetimeScopeProvider scopeProvider;

        [Fact]
        public void ExecuteResult_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.JsVersion).Returns("123");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.Content(It.IsAny<string>())).Returns("http://localhost:65181/V1/Portal/Abc");

            // Action
            var versionLink = UrlHelperExtension.VersionedLink(urlHelper.Object, "~/Abc");

            // Assert
            Assert.Equal("http://localhost:65181/V1/Portal/Abc?v=123", versionLink);
        }
    }
}
