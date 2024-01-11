using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Logging;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Fanex.Affiliate.Admin.Core.UnitTests.TestHelper
{
    public static class MockingHelper
    {
        public static ISessionService GetSessionService()
        {
            return Mock.Of<ISessionService>(ss =>
                ss.AffName == Constant.AffName &&
                ss.OnUserId == Constant.OnUserId &&
                ss.UserId == Constant.UserId &&
                ss.UserPermissionList == new List<UserPermission>
                {
                    new UserPermission
                    {
                        IsHost = true,
                        FunctionName = Function.RevenueGroup
                    }
                }
            );
        }

        public static HttpRequestBase GetHttpRequest()
        {
            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Setup(p => p.ApplicationPath).Returns("/");
            mockHttpRequest.Setup(p => p.UserHostAddress).Returns("::1");
            mockHttpRequest.Setup(p => p.Url).Returns(new Uri(Constant.DefaultRootDomain));
            mockHttpRequest.Setup(p => p.ServerVariables).Returns(new NameValueCollection());
            mockHttpRequest.Setup(p => p.Browser.Browser).Returns(Constant.Browser);

            return mockHttpRequest.Object;
        }

        public static void SetUpLogEngineEnviroment()
        {
            var logEngine = Substitute.For<ILogEngine>();

            LogManager.SetDefaultLogCategory("Tests")
                .Use(new List<ILogEngine>
                {
                    logEngine
                });
        }

        public static void SetUpHttpContextSession()
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
