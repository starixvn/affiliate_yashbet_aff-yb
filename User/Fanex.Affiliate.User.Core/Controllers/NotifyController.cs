using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class NotifyController : BaseController
    {
        private readonly IAppSetting _appSetting;

        public NotifyController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService)
            : base(sessionService, languageService)
        {
            _appSetting = appSetting;
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View("~/Views/Notify/Error.cshtml");
        }

        [HttpGet]
        public ActionResult PageNotFound()
        {
            return View("~/Views/Notify/PageNotFound.cshtml");
        }

        public ActionResult CodeExpired()
        {
            return Json(new
            {
                IsOk = false,
                IsExpired = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
