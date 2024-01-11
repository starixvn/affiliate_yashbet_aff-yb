using Affiliate.YB.WebApi.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Web.Http;

namespace Affiliate.YB.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"]);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "FakeApi",
                routeTemplate: "api/Account/{action}/{id}",
                defaults: new { controller = "Fake", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var formaters = GlobalConfiguration.Configuration.Formatters;

            var jsonFormater = formaters.JsonFormatter;
            jsonFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormater.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            formaters.Remove(formaters.XmlFormatter);

            if (!isTest)
            {
                config.Filters.Add(new HmacAuthorizationFilter(ConfigurationManager.AppSettings["AuthenticationKey"]));
            }
            config.Filters.Add(new GlobalExceptionFilter());
        }
    }
}