using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessionService = DependencyResolver.Current.GetService<ISessionService>();
            var isAuthorized = base.AuthorizeCore(httpContext)
                && httpContext.Request.IsAuthenticated
                && httpContext.User.Identity.IsAuthenticated;
            return isAuthorized
                && sessionService.UserId > 0
                && sessionService.UserPermissionList != null
                && sessionService.UserPermissionList.Any();
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            FormsAuthentication.SignOut();
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.HttpContext.Response.ContentType = "application/json";
                filterContext.Result = new JsonResult
                {
                    Data = new { ErrCode = ErrCode.ErrCodeSessionExpired, ErrMsg = "Your session was expired! Please login again." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Portal/Login");
            }
        }
    }
}