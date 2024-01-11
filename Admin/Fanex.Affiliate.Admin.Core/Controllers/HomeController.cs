using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Home;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    public class HomeController : ReadOnlySessionBaseController
    {
        private readonly IHomeService _homeService;

        public HomeController(
            ISessionService sessionService,
            IHomeService homeService) : base(sessionService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetGeneralInfo()
        {
            return GenerateJsonNetResult(_homeService.GetGeneralInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetOnlineList(int pageIndex = 1, int pageSize = 20)
        {
            return GenerateJsonNetResult(_homeService.GetOnlineList(pageIndex, pageSize));
        }
    }
}