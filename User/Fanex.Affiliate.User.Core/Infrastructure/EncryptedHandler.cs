using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class EncryptedHandler : IHttpHandler, IRequiresSessionState
    {
        private readonly RequestContext _requestContext;
        private readonly IHttpRequestParserHandler _httpRequestParserHandle;
        private const int DeniedByFilterSubStatus = 5;

        public EncryptedHandler(RequestContext requestContext)
            : this(requestContext, new HttpRequestParserHandler(requestContext.HttpContext.Request))
        {
        }

        public EncryptedHandler(RequestContext requestContext, IHttpRequestParserHandler parserHanlder)
        {
            _requestContext = requestContext;
            _httpRequestParserHandle = parserHanlder;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var encryptedQuery = _requestContext.HttpContext.Request.QueryString["callInfo"]?.Trim();
                var parseQueryResult = _httpRequestParserHandle.TryParseQueryString(encryptedQuery);

                if (!parseQueryResult.IsValid && parseQueryResult.IsExpired)
                {
                    var response = _requestContext.HttpContext.Response;
                    var request = WrapRequest(context.Request);
                    if (request.IsAjaxRequest())
                    {
                        var controller = ControllerBuilder.Current.GetControllerFactory()
                                    .CreateController(_requestContext, "Notify");
                        _requestContext.RouteData.Values["action"] = "CodeExpired";
                        controller.Execute(_requestContext);
                    }
                    else
                    {
                        response.Redirect(ToAbsolutePath("~/Notify/Error"));
                    }

                    return;
                }
                else if (!parseQueryResult.IsValid)
                {
                    SetNotFoundResponse(DeniedByFilterSubStatus);
                }
                else
                {
                    var controller = ControllerBuilder.Current.GetControllerFactory()
                                    .CreateController(_requestContext, parseQueryResult.ControllerName);
                    if (controller != null)
                    {
                        _requestContext.RouteData.Values["action"] = parseQueryResult.ActionName;
                        controller.Execute(_requestContext);
                    }
                }
            }
            catch (Exception)
            {
                SetNotFoundResponse();
            }
        }

        private void SetNotFoundResponse(int subStatus = 0)
        {
            _requestContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            _requestContext.HttpContext.Response.SubStatusCode = subStatus;
        }

        protected virtual HttpRequestBase WrapRequest(HttpRequest request)
            => new HttpRequestWrapper(request);

        protected virtual string ToAbsolutePath(string virtualPath)
            => VirtualPathUtility.ToAbsolute(virtualPath);
    }
}