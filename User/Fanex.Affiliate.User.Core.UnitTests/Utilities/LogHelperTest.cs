using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Utilities
{
    public class LogHelperTest
    {
        [Fact]
        public void WriteInfo_WhenCall_WriteLog()
        {
            MockingHelper.SetUpLogEngineEnviroment();
            LogHelper.WriteInfo("2");

            Assert.True(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Message")]
        public void WriteError_WhenCall_WriteLog(string additionalMessage)
        {
            SetUpEnviroment();
            MockingHelper.SetUpLogEngineEnviroment();
            var ex = Substitute.For<Exception>();

            LogHelper.WriteError(ex, additionalMessage);

            Assert.True(true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Non-substitutable member", "NS1000:Non-virtual setup specification.", Justification = "<Pending>")]
        private void SetUpEnviroment()
        {
            var request = new HttpRequest("111", "http://127.0.0.1:8080/controller/action", string.Empty);
            var test = Substitute.For<HttpBrowserCapabilities>();
            test.Type.Returns("");
            test.Version.Returns("");
            request.Browser = test;

            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                 new HttpStaticObjectsCollection(), 10, true,
                 HttpCookieMode.AutoDetect,
                 SessionStateMode.InProc, false);
            SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current, sessionContainer);
        }
    }
}
