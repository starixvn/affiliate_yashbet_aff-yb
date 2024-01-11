using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ValidateAntiForgeryHeaderAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KEYNAME = "__RequestVerificationToken";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            string clientToken = request.IsAjaxRequest() ? request.Headers.Get(KEYNAME) : request.Form[KEYNAME];
            if (clientToken == null)
            {
                throw new HttpAntiForgeryException($"Header/Form does not contain {KEYNAME}");
            }

            var serverTokenCookie = request.Cookies.Get(AntiForgeryConfig.CookieName);
            if (serverTokenCookie == null || serverTokenCookie.Value == null)
            {
                throw new HttpAntiForgeryException($"Cookies does not contain {KEYNAME}");
            }

            AntiForgery.Validate(serverTokenCookie.Value, clientToken);
        }


    }
}
