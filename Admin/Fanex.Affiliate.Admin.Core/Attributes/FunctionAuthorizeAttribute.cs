using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FunctionAuthorizeAttribute : ActionFilterAttribute
    {
        public string FunctionName { get; set; }

        public string FunctionNameOption { get; set; }

        public bool IsHost { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            var isSkipCheckAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowByPassFunctionAuthorizeAttribute), true);

            if (!string.IsNullOrEmpty(FunctionName) && !isSkipCheckAuthorization)
            {
                var hasPerimission = HasPerimission();

                if (!hasPerimission)
                {
                    if (context.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        filterContext.HttpContext.Response.ContentType = "application/json";
                        filterContext.Result = new JsonResult
                        {
                            Data = new { ErrCode = ErrCode.ErrCodeAccessDenied, ErrMsg = "Access Denied." },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        context.Response.Redirect("~/Portal/AccessDenied");
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private bool HasPerimission()
        {
            var sessionService = DependencyResolver.Current.GetService<ISessionService>();
            var permissionList = sessionService.UserPermissionList;

            return permissionList.Any(i => (i.FunctionName.MatchWith(this.FunctionName) || i.FunctionName.MatchWith(this.FunctionNameOption))
                                    && (i.IsHost || i.IsHost == IsHost));
        }
    }
}