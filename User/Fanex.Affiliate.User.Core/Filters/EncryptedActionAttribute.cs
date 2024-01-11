using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class EncryptedActionAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.RawUrl.StartsWith(VirtualPathUtility.ToAbsolute("~/Execute"));
        }
    }
}
