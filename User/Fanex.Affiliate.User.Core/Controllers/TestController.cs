namespace Fanex.Affiliate.User.Core.Controllers
{
    using BusinessService;
    using Constants;
    using CoreService;
    using DTO;
    using Enum;
    using Fanex.Affiliate.User.Core.Resources;
    using Filters;
    using MetalModels.Email;
    using ONEbook.UM.AspNetMVC.Filters;
    using System;
    using System.Web.Mvc;
    using Utilities;
    using ViewModels;

    [CheckUMFilter]
    [CustomAuthorize]
    public class TestController : BaseController
    {
        private readonly IMailProcessor _mailProcessor;
        private readonly IAppSetting _appSetting;

        private readonly string _mailContact;
        private readonly string _userName;
        private const int AffId = 123456;

        public TestController(ISessionService sessionService,
            ILanguageService languageService, IAppSetting appSetting, IMailProcessor mailProcessor) : base(sessionService, languageService)
        {
            _mailProcessor = mailProcessor;
            _appSetting = appSetting;
            _mailContact = appSetting.GetMailContactByLanguageCode(sessionService.UserSession?.PreferredLanguage ?? sessionService.CurrentLanguage);
            _userName = sessionService.UserSession?.UserName ?? "NovaAff0000";
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!IsValidUser())
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpGet]
        public ActionResult Preview(TestCriteria criteria)
        {
            if (!IsValidUser())
            {
                return HttpNotFound();
            }

            try
            {
                SetLanguage(criteria);
                ViewBag.Data = GetEmailContent(criteria);
                LanguageService.SetCulture(SystemLanguage.EnglishLanguageCode);
            }
            catch (Exception ex)
            {
                ViewBag.Data = ex.Message + "\n" + ex.StackTrace;
            }

            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(TestCriteria criteria)
        {
            if (!IsValidUser())
            {
                return HttpNotFound();
            }

            var success = true;
            var message = string.Empty;

            try
            {
                SetLanguage(criteria);
                var content = GetEmailContent(criteria);
                string Subject = GetSubject(criteria.Function);

                _mailProcessor.Send(content, criteria.EmailAddress, Subject, criteria.Language);

                LanguageService.SetCulture(SystemLanguage.EnglishLanguageCode);
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message + "\n" + ex.StackTrace;
            }

            var result = new
            {
                Success = success,
                Message = message
            };

            return Json(result);
        }

        private void SetLanguage(TestCriteria criteria)
        {
            criteria.Language = string.Equals("Csd", criteria.Function) ? SystemLanguage.EnglishLanguageCode : criteria.Language;
            LanguageService.SetCulture(criteria.Language);
        }

        private string GetEmailContent(TestCriteria criteria)
        {
            var content = GetEmailTextContent(criteria);

            return criteria.Format == "txt" ? content : GetEmailHtmlContent(content, criteria.Language);
        }

        private string GetEmailTextContent(TestCriteria criteria)
        {
            var language = criteria.Language;
            var function = criteria.Function;

            var viewPath = string.Format(TemplateString.EmailTemplatePath, function, language);
            var viewPathBackup = string.Format(TemplateString.EmailTemplatePath, function, _appSetting.DefaultLanguageCode);
            var physicalPath = Server.MapPath(viewPath);
            var physicalPathBackup = Server.MapPath(viewPathBackup);
            var model = GetModelByFunction(function);

            return model == null ? string.Empty : _mailProcessor.BuildTemplateContent(physicalPath, physicalPathBackup, model);
        }

        private string GetEmailHtmlContent(string content, string language)
        {
            var domain = SiteDomainHelper.GetRootUrl(this, _appSetting);

            var viewModel = new EmailTemplateViewModel
            {
                RootUrl = domain,
                ImageRootUrl = string.Format(TemplateString.EmailImageRootPath, domain),
                Language = language,
                Content = content,
                Title = "This is title " + language
            };
            var viewPath = Server.MapPath("~/Views/Shared/Email/CommonLayout.cshtml");
            var viewPathBackup = viewPath;

            return _mailProcessor.BuildTemplateContent(viewPath, viewPathBackup, viewModel);
        }

        private static string GetSubject(string functionName)
        {
            switch (functionName)
            {
                case "Csd":
                    return Lang.CsdSubject;

                case "RecoverPassword":
                    return Lang.SubjectForgetPass;

                case "PaymentCancel":
                    return string.Format(Lang.PaymentRequestCancelSubject, AffId);

                case "PaymentRequest":
                    return string.Format(Lang.PaymentRequestSubject, AffId);

                case "NewPassword":
                    return Lang.SubjectNewPass;

                case "VerificationAccount":
                    return Lang.SubjectVerificationAccount;

                default:
                    return Lang.AffiliateProgram;
            }
        }

        private object GetModelByFunction(string functionName)
        {
            switch (functionName)
            {
                case "Csd":
                    return GetCSDModel();

                case "RecoverPassword":
                    return GetForgetPasswordModel();

                case "PaymentCancel":
                    return GetPaymentModel(PaymentRequestType.Cancel);

                case "PaymentRequest":
                    return GetPaymentModel(PaymentRequestType.Create);

                case "NewPassword":
                    return GetRecoverPasswordModel();

                case "VerificationAccount":
                    return GetVerificationAccountModel();

                default:
                    return null;
            }
        }

        private object GetCSDModel()
        {
            return new NoticeEmailParam
            {
                UserName = _userName,
                FirstName = "Firs Name",
                LastName = "Last Name",
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                SiteUrl = "abc.com.vn",
                MoreSiteUrls = "xyz.com.vn",
                MailContact = _mailContact
            };
        }

        private object GetForgetPasswordModel()
        {
            return new RecoverPasswordEmailParam
            {
                User = GetUserInfo(),
                MailContact = _mailContact,
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                Code = "12345",
                RootDomain = SiteDomainHelper.GetRootUrl(this, _appSetting),
                MailTo = _mailContact
            };
        }

        private object GetPaymentModel(PaymentRequestType type)
        {
            const long TranId = 1L;
            const decimal Amount = 989;

            return new PaymentEmailParam
            {
                User = GetUserInfo(),
                MailContact = _mailContact,
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                DefaultCurrency = _appSetting.DefaultCurrency,
                TransId = TranId,
                RequestDate = DateTimeHelper.NowDateTime.Date,
                Amount = Amount,
                RootDomain = SiteDomainHelper.GetRootUrl(this, _appSetting),
                MailTo = _mailContact,
                PaymentType = type
            };
        }

        private object GetRecoverPasswordModel()
        {
            return new NewPasswordEmailParam
            {
                User = GetUserInfo(),
                MailContact = _mailContact,
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                NewPassword = "1234aa",
                RootDomain = SiteDomainHelper.GetRootUrl(this, _appSetting),
                MailTo = _mailContact,
            };
        }

        private object GetVerificationAccountModel()
        {
            return new VerificationAccountEmailParam
            {
                User = GetUserInfo(),
                MailContact = _mailContact,
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                RootDomain = SiteDomainHelper.GetRootUrl(this, _appSetting),
                Code = "123456",
                Ip = "127.0.0.1"
            };
        }

        private UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                AffId = AffId,
                FirstName = "Fisrt name",
                UserName = _userName,
                Email = "abc@nova.com",
                LastName = "Last name",
                MailOption = false,
                PreferredLanguage = SystemLanguage.EnglishLanguageCode
            };
        }

        private bool IsValidUser()
        {
            return _appSetting.IsDev || _appSetting.IsLocal || SessionService.IsInternal;
        }
    }

    public class TestCriteria
    {
        public string Language { get; set; }

        public string Function { get; set; }

        public string Format { get; set; }

        public string EmailAddress { get; set; }
    }
}