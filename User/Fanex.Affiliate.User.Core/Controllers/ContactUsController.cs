namespace Fanex.Affiliate.User.Core.Controllers
{
    using BusinessService;
    using CoreService;
    using System.Web.Mvc;
    using Utilities;

    public class ContactUsController : BaseController
    {
        private readonly IAppSetting _appSetting;

        public ContactUsController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var mailContact = _appSetting.MailContact;

            return View($"~/Views/ContactUs/Index.cshtml", model: mailContact);
        }
    }
}