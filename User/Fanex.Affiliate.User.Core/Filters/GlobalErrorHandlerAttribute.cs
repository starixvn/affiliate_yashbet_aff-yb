using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Affiliate.User.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class GlobalErrorHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            var httpException = exception as HttpException;
            if (filterContext.ExceptionHandled || (httpException != null && httpException.GetHttpCode() == (int)HttpStatusCode.NotFound))
            {
                return;
            }

            var httpContext = filterContext.RequestContext.HttpContext;

            if (!(exception is HttpAntiForgeryException))
            {
                LogHelper.WriteError(exception);
            }

            if (httpContext.Request.IsAjaxRequest())
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                filterContext.Result = new JsonResult
                {
                    Data = new { Success = false, Message = Error.General },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var routeValue = new
                {
                    controller = "Notify",
                    action = "Error"
                };

                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(routeValue));
            }

            filterContext.ExceptionHandled = true;
        }
    }
}
