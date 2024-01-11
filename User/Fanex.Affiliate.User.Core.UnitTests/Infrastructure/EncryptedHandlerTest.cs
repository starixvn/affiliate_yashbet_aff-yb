using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.Infrastructure;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Infrastructure
{
    public class EncryptedHandlerTest
    {
        private readonly RequestContext RequestContext;
        private readonly IHttpRequestParserHandler HttpRequestParserHandler;
        private readonly IControllerFactory ControllerFactory;
        private HttpRequestBase HttpRequest;

        public EncryptedHandlerTest()
        {
            RequestContext = Substitute.For<RequestContext>();
            HttpRequestParserHandler = Substitute.For<IHttpRequestParserHandler>();
            ControllerFactory = Substitute.For<IControllerFactory>();
        }

        [Fact]
        public void ProcessRequest_WhenParseQueryStringFail_ResponseNotFound()
        {
            var handler = CreateHandler();
            SetUpRequestContext();
            HttpRequestParserHandler.TryParseQueryString(null)
                .ReturnsForAnyArgs(new DTO.VerificationAccount.QueryStringResult
                {
                    IsValid = false
                });

            handler.ProcessRequest(CreateParam());

            Assert.Equal((int)HttpStatusCode.NotFound, RequestContext.HttpContext.Response.StatusCode);
            Assert.Equal(5, RequestContext.HttpContext.Response.SubStatusCode);
        }

        [Fact]
        public void ProcessRequest_WhenHaveExecption_ResponseNotFount()
        {
            var handler = CreateHandler();
            SetUpRequestContext();
            HttpRequestParserHandler.TryParseQueryString(null)
                .ReturnsForAnyArgs(new DTO.VerificationAccount.QueryStringResult
                {
                    IsValid = false
                });
            RequestContext.HttpContext.Request.ThrowsForAnyArgs(new Exception());

            handler.ProcessRequest(CreateParam());

            Assert.Equal((int)HttpStatusCode.NotFound, RequestContext.HttpContext.Response.StatusCode);
            Assert.Equal(0, RequestContext.HttpContext.Response.SubStatusCode);
        }

        [Fact]
        public void ProcessRequest_WhenRequestIsExpiredAndIsNotAjaxRequest_ResponseRedirectErrorSite()
        {
            var handler = CreateHandler();
            SetUpRequestContext();
            HttpRequestParserHandler.TryParseQueryString(null)
                .ReturnsForAnyArgs(new DTO.VerificationAccount.QueryStringResult
                {
                    IsValid = false,
                    IsExpired = true
                });

            handler.ProcessRequest(CreateParam());

            var reponse = RequestContext.HttpContext.Response;
            reponse.Received().Redirect(Arg.Is<string>(s => s.Contains("Notify/Error")));
        }

        [Fact]
        public void ProcessRequest_WhenRequestIsExpiredAndIsAjaxRequest_ChangeContextToNoifyCodeExpired()
        {
            var handler = CreateHandler();
            SetUpRequestContext();
            HttpRequestParserHandler.TryParseQueryString(null)
                .ReturnsForAnyArgs(new DTO.VerificationAccount.QueryStringResult
                {
                    IsValid = false,
                    IsExpired = true
                });
            NSubtituteMockingHelper.FakeAjaxRequest(HttpRequest);
            RequestContext.RouteData = new RouteData();
            SetUpControllerBuilder();

            handler.ProcessRequest(CreateParam());

            ControllerFactory.Received().CreateController(Arg.Any<RequestContext>(), "Notify");
            Assert.Equal("CodeExpired", RequestContext.RouteData.Values["action"]);
        }

        [Fact]
        public void ProcessRequest_WhenRequestIsValidAndNotExpired_ExecuteContext()
        {
            var handler = CreateHandler();
            SetUpRequestContext();
            var parseResult = new QueryStringResult
            {
                IsExpired = false,
                ActionName = "test",
                ControllerName = "test",
                IsValid = true
            };
            HttpRequestParserHandler.TryParseQueryString(null)
                .ReturnsForAnyArgs(parseResult);
            RequestContext.RouteData = new RouteData();
            SetUpControllerBuilder();

            handler.ProcessRequest(CreateParam());

            ControllerFactory.Received().CreateController(Arg.Any<RequestContext>(), parseResult.ControllerName);
            Assert.Equal(parseResult.ActionName, RequestContext.RouteData.Values["action"]);
        }

        private void SetUpControllerBuilder()
        {
            ControllerBuilder.Current.SetControllerFactory(ControllerFactory);
        }

        private void SetUpRequestContext()
        {
            var context = Substitute.For<HttpContextBase>();
            var request = NSubtituteMockingHelper.GetHttpRequest();
            var response = Substitute.For<HttpResponseBase>();
            request.QueryString.Returns(new System.Collections.Specialized.NameValueCollection());
            context.Request.Returns(request);
            context.Response.Returns(response);
            RequestContext.HttpContext = context;
        }

        private EncryptedHandler CreateHandler()
        {
            HttpRequest = NSubtituteMockingHelper.GetHttpRequest();
            var handler = new EncryptedHandlerFake(RequestContext, HttpRequestParserHandler);
            handler.HttpRequest = HttpRequest;
            return handler;
        }

        private HttpContext CreateParam()
        {
            var request = new HttpRequest("", "http://abc.com", "");
            var response = new HttpResponse(new StringWriter());
            return new HttpContext(request, response);
        }
    }

    internal class EncryptedHandlerFake : EncryptedHandler
    {
        public HttpRequestBase HttpRequest { get; set; } = NSubtituteMockingHelper.GetHttpRequest();

        public EncryptedHandlerFake(RequestContext requestContext, IHttpRequestParserHandler parserHanlder)
            : base(requestContext, parserHanlder) { }

        protected override string ToAbsolutePath(string virtualPath)
            => virtualPath;

        protected override HttpRequestBase WrapRequest(HttpRequest request)
            => HttpRequest;
    }
}
