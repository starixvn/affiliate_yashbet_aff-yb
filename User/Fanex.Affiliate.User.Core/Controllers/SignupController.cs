namespace Fanex.Affiliate.User.Core.Controllers
{
    using BusinessService;
    using CoreService;
    using DTO;
    using Fanex.Affiliate.User.Core.Extensions;
    using MetalModels;
    using MetalModels.Email;
    using Newtonsoft.Json;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Utilities;

    public class SignupController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IAppSetting _appSetting;
        private readonly ICaptchaService _captchaService;
        private readonly ISecurityQuestionService _securityQuestionService;
        private const string ParentCodeCookieName = "Fanex.UserSite.RefCode";
        private const int CookieTimeOut = 6;

        public SignupController(
            ISessionService sessionService,
            IAppSetting appSetting,
            IAccountService accountService,
            ILanguageService languageService,
            ICaptchaService captchaService,
            ISecurityQuestionService securityQuestionService) : base(sessionService, languageService)
        {
            _accountService = accountService;
            _captchaService = captchaService;
            _appSetting = appSetting;
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet]
        public ActionResult Index(string refId = null, string affCode = null, string affId = null)
        {
            if (string.IsNullOrEmpty(affCode) && !string.IsNullOrEmpty(affId))
            {
                affCode = affId;
            }

            if ((string.IsNullOrEmpty(refId) || string.Equals(refId, "index", StringComparison.CurrentCultureIgnoreCase)) && string.IsNullOrEmpty(affCode))
            {
                return View("~/Views/Signup/Index.cshtml", new UserSignUpParam());
            }

            var affInfo = new GeneralAffInfo();

            if (int.TryParse(refId, out int parentAffId))
            {
                affInfo = _accountService.GetGeneralAffInfo(affId: parentAffId);
            }

            if (int.TryParse(affCode, out _))
            {
                affInfo = _accountService.GetGeneralAffInfo(affCode: affCode);
            }

            if (affInfo.IsValidAff)
            {
                SetCookie(affInfo.AffCode);

                return View("~/Views/Signup/Index.cshtml", new UserSignUpParam());
            }

            return View("~/Views/Signup/Index.cshtml", new UserSignUpParam { IsValidParentCode = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(UserSignUpParam param)
        {
            var result = new ResultInfo { IsOk = false };
            var affParentCode = HttpContext.Request.Cookies[ParentCodeCookieName]?.Value;
            if (!_captchaService.IsValid(param.Captcha, SessionService.SignUpCaptchaCode, Request.GetClientIP()))
            {
                result.Message = Lang.ValidationCode;
            }
            else if (!IsValidAppContactList(param.ContactAppList))
            {
                result.Message = Error.RequireIMAccount;
            }
            else if (ModelState.IsValid)
            {
                var signUpParam = new UserSignUpParam
                {
                    FirstName = Server.HtmlEncode(param.FirstName),
                    LastName = Server.HtmlEncode(param.LastName),
                    Email = Server.HtmlEncode(param.Email),
                    Phone = Server.HtmlEncode(param.Phone),
                    Address = Server.HtmlEncode(param.Address),
                    City = Server.HtmlEncode(param.City),
                    Postal = Server.HtmlEncode(param.Postal),
                    Country = Server.HtmlEncode(param.Country),
                    SiteUrl = Server.HtmlEncode(param.SiteUrl),
                    MoreSiteUrls = Server.HtmlEncode(param.MoreSiteUrls),
                    NoSiteDesc = Server.HtmlEncode(param.NoSiteDesc),
                    PreferredLanguage = Server.HtmlEncode(param.PreferredLanguage),
                    QuestionId = param.QuestionId,
                    Answer = Server.HtmlEncode(param.Answer),
                    Player_UserName = Server.HtmlEncode(param.Player_UserName),
                    ContactApps = JsonConvert.SerializeObject(param.ContactAppList.Select(p => new ContactApp
                    {
                        Name = Server.HtmlEncode(p.Name),
                        Value = Server.HtmlEncode(p.Value),
                        Order = p.Order
                    })),
                    Gender = param.Gender,
                    State = Server.HtmlEncode(param.State),
                    Language = SessionService.CurrentLanguage,
                    AffType = _appSetting.AllowedAffiliateTypes.FirstOrDefault(),
                    Ip = Request.GetClientIP(),
                    IsLocal = _appSetting.IsLocal,
                    AffParentCode = affParentCode
                };

                var mailContact = _appSetting.MailContact;
                var emailParam = new EmailBaseParam
                {
                    MailContact = mailContact,
                    RootDomain = SiteDomainHelper.GetRootUrl(this, _appSetting)
                };

                result = await _accountService.CreateAffiliateAsync(signUpParam, emailParam);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                result.Message = string.Join("<br>", errors);
            }

            return Json(result);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult SecurityQuestion()
        {
            var model = _securityQuestionService.GetSecurityQuestions(SessionService.CurrentLanguage);

            return PartialView("~/Views/Signup/_SecurityQuestion.cshtml", model);
        }

        private static bool IsValidAppContactList(IEnumerable<ContactApp> contactAppList)
        {
            return contactAppList != null && !contactAppList.All(c => string.IsNullOrEmpty(c.Value));
        }

        private void SetCookie(string parentAffCode)
        {
            var cookie = new HttpCookie(ParentCodeCookieName)
            {
                HttpOnly = true,
                Expires = DateTimeHelper.NowDateTime.AddHours(CookieTimeOut),
                Value = parentAffCode
            };

            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}