using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    public class SubAffMediaGalleryController : BaseMarketingController
    {
        public SubAffMediaGalleryController(ISessionService sessionService,
            IAppSetting appSetting,
            ILanguageService languageService,
            IMarketingService marketingService)
            : base(sessionService,
                appSetting,
                languageService,
                marketingService)
        {
        }

        public ActionResult Index()
        {
            return RedirectToRoute("UserPanel", new { controller = "MediaGallery", isTargetToMemberSite = false });
        }
    }
}
