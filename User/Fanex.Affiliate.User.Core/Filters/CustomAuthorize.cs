using Fanex.Affiliate.User.Core.CoreService;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Affiliate.User.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var sessionService = DependencyResolver.Current.GetService<ISessionService>();

            var skipAuthorization = CheckExistAttribute(filterContext, typeof(AllowAnonymousAttribute));
            var context = filterContext.RequestContext.HttpContext;
            base.OnAuthorization(filterContext);

            if (!skipAuthorization)
            {
                var authorized = sessionService.UserSession != null && context.Request.IsAuthenticated
                    && context.User.Identity.IsAuthenticated;

                if (!authorized)
                {
                    if (context.Request.IsAjaxRequest())
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        filterContext.Result = new JsonResult
                        {
                            Data = new { ErrCode = HttpStatusCode.Forbidden, ErrMsg = "UnAuthorized" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        var routeValue = new
                        {
                            controller = "Home",
                            action = "Index",
                        };

                        filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(routeValue));
                    }

                    context.Session.Clear();
                    context.Response.Cookies.Clear();

                }
            }


        }

        private static bool CheckExistAttribute(AuthorizationContext filterContext, Type attrType)
            => filterContext.ActionDescriptor.IsDefined(attrType, true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(attrType, true);
    }
}
