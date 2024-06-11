namespace Affiliate.Banner
{
    using System.Web.Mvc;
    using ONEbook.UM.AspNetMVC.Filters;

    [CheckUMFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
