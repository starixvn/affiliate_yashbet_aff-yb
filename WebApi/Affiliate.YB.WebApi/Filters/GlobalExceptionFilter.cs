using Fanex.Logging.Extensions.Web;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Affiliate.YB.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                var exception = actionExecutedContext.Exception;
                ExceptionHandler.Handle(exception);
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, new HttpError(exception, true));
            }
        }
    }
}