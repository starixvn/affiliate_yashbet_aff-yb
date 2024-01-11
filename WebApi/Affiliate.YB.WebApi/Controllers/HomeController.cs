using Affiliate.YB.DTO;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Affiliate.YB.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamicRepository dynamicRepository;

        public HomeController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public ActionResult Index()
        {
            var connectedMessage = "Database connects successfully";
            try
            {
                dynamicRepository.Fetch<AffCustomerInfo>(new GetAffCustomersCriteria(DateTime.Now, "Slave"));
            }
            catch (Exception ex)
            {
                connectedMessage = ex.ToString();
            }

            var env = ConfigurationManager.AppSettings.Get("Enviroment");

            return new ContentResult
            {
                Content = $"Site is running {env} <br/> {connectedMessage}",
                ContentType = "text/html"
            };
        }
    }
}