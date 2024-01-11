using System;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AllowByPassFunctionAuthorizeAttribute : ActionFilterAttribute
    {
    }
}
