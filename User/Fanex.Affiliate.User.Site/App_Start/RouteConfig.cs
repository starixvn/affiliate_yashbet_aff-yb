using Fanex.Affiliate.User.Core.Infrastructure;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Affiliate.User.Site
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            const string IndexAction = "Index";
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(new HandleLegacyUrlRoute(new Dictionary<string, UrlSchema> {
                {
                    @"/default.aspx",new UrlSchema{Controller="Home",Action=IndexAction }
                },
                {
                    @"/index.aspx",new UrlSchema{Controller="Home",Action=IndexAction }
                },
                {
                    @"/programdetails.aspx",new UrlSchema{Controller="RevenueSharingStructure",Action=IndexAction}
                },
                {
                    @"/condition.aspx",new UrlSchema{Controller="TermAndCondition",Action=IndexAction}
                },
                {
                    @"/faqs.aspx",new UrlSchema{Controller="FAQ",Action=IndexAction}
                },
                {
                    @"/contactus.aspx",new UrlSchema{Controller="ContactUs",Action=IndexAction}
                },
                {
                    @"/forgetpass.aspx",new UrlSchema{Controller="Password",Action=IndexAction}
                },
                {
                    @"/signup.aspx",new UrlSchema{Controller="Signup",Action=IndexAction}
                }
            }));
            var encryptedRoute = new Route("Execute", new EncryptedRouteHandler());
            routes.Add(encryptedRoute);

            routes.MapRoute(
              name: "UserPanel",
              url: "UserPanel/{controller}/{action}/{id}",
              defaults: new { controller = "AccountOverview", action = IndexAction, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SignUpUsingAffID",
                url: "Signup/{refId}/{action}",
                defaults: new { controller = "Signup", action = IndexAction, refId = UrlParameter.Optional },
                namespaces: new[] { "Fanex.Affiliate.User.Core.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = IndexAction, id = UrlParameter.Optional }
            );
        }
    }
}