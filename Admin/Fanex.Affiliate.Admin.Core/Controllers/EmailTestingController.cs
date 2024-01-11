using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [FunctionAuthorize(FunctionName = Function.UserControl)]
    public class EmailTestingController : ReadOnlySessionBaseController
    {
        private const int _transId = 123;
        private const int _amount = 100;
        private const string _firstName = "First Name";

        private readonly IMailProcessor _mailProcessor;

        private readonly IAppSetting _appSetting;

        private readonly ILanguageService _languageService;

        public EmailTestingController(
            ISessionService sessionService,
            IMailProcessor mailProcessor,
            IAppSetting appSetting,
            ILanguageService languageService
            ) : base(sessionService)
        {
            _mailProcessor = mailProcessor;
            _appSetting = appSetting;
            _languageService = languageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Preview(string language, string function, string format)
        {
            string result;
            try
            {
                _languageService.SetCulture(language);
                result = GetEmailContent(language, function, format);
                _languageService.SetCulture("en-US");
            }
            catch (Exception ex)
            {
                result = ex.Message + "\n" + ex.StackTrace;
            }

            return View(model: result);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.UserControl, IsHost = true)]
        public ActionResult SendEmail(string language, string function, string format, string emailAddress)
        {
            var success = true;
            var message = string.Empty;

            try
            {
                _languageService.SetCulture(language);
                var content = GetEmailContent(language, function, format);
                var shortedLanguageCode = GetShortedLanguageCode(language);

                var subject = GetSubject(function);

                _mailProcessor.Send(content, emailAddress, subject, shortedLanguageCode);
                _languageService.SetCulture("en-US");
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

            return GenerateJsonNetResult(result);
        }

        private string GetEmailContent(string language, string function, string format)
        {
            var content = GetEmailTextContent(function, language);

            return format == "txt" ? content : GetEmailHtmlContent(content, language);
        }

        private string GetEmailTextContent(string function, string language)
        {
            var viewPath = string.Format(TemplateString.EmailTemplatePath, function, language);
            var viewPathBackup = string.Format(TemplateString.EmailTemplatePath, function, _appSetting.DefaultLanguageCode);
            var physicalPath = Server.MapPath(viewPath);
            var physicalPathBackup = Server.MapPath(viewPathBackup);
            var model = GetModelByFunction(function, language);

            return model == null ? string.Empty : _mailProcessor.BuildTemplateContent(physicalPath, physicalPathBackup, model);
        }

        private string GetEmailHtmlContent(string content, string language)
        {
            var domain = SiteDomainHelper.GetRootUrl(this);

            var viewModel = new EmailTemplateViewModel
            {
                RootUrl = domain,
                UserSiteUrl = "https://aff-yb.nexdev.net/YBUser",
                ImageRootUrl = string.Format(TemplateString.EmailImageRootPath, domain),
                Language = language,
                Content = content,
                Title = "This is title " + language
            };
            var viewPath = Server.MapPath("~/Areas/Portal/Views/Shared/Email/CommonLayout.cshtml");
            var viewPathBackup = viewPath;

            return _mailProcessor.BuildTemplateContent(viewPath, viewPathBackup, viewModel);
        }

        private static string GetSubject(string functionName)
        {
            switch (functionName)
            {
                case "AffiliateDisapproved":
                    return Lang.EmailSubjectAffDisapproved;

                case "AffiliateInformation":
                    return Lang.EmailSubjectAffInfor;

                case "PaymentRequest/Approved":
                    return string.Format(Lang.EmailSubjectPaymentRequestApproved, _transId);

                case "PaymentRequest/Rejected":
                    return string.Format(Lang.EmailSubjectPaymentRequestRejected, _transId);

                default:
                    return Lang.Affiliate;
            }
        }

        private object GetModelByFunction(string functionName, string languageCode)
        {
            var shortedLanguageCode = GetShortedLanguageCode(languageCode);

            var mailContact = _appSetting.GetMailContactByShortedLanguageCode(shortedLanguageCode);

            switch (functionName)
            {
                case "AffiliateDisapproved":
                    return new AffInformationEmail
                    {
                        MailContact = mailContact,
                        SiteFriendlyName = _appSetting.SiteFriendlyName,
                        UserSiteUrl = _appSetting.GetAffSiteUrlByShortedLanguageCode(shortedLanguageCode),
                        FirstName = _firstName
                    };
                case "AffiliateInformation":
                    return new AffInformationEmail
                    {
                        MailContact = mailContact,
                        SiteFriendlyName = _appSetting.SiteFriendlyName,
                        UserSiteUrl = _appSetting.GetAffSiteUrlByShortedLanguageCode(shortedLanguageCode),
                        FirstName = _firstName,
                        UserName = "NOVA00000",
                        Password = "qwrfsdg"
                    };
                case "PaymentRequest/Approved":
                    return new PaymentRequestUpdateEmail
                    {
                        MailContact = mailContact,
                        SiteFriendlyName = _appSetting.SiteFriendlyName,
                        DefaultCurrency = _appSetting.DefaultCurrency,
                        FirstName = _firstName,
                        TransId = _transId,
                        TransDate = DateTimeHelper.NowDateTimeOffset.ToString("MM/dd/yyyy hh:mm:ss"),
                        Amount = _amount
                    };
                case "PaymentRequest/Rejected":
                    return new PaymentRequestUpdateEmail
                    {
                        MailContact = mailContact,
                        SiteFriendlyName = _appSetting.SiteFriendlyName,
                        DefaultCurrency = _appSetting.DefaultCurrency,
                        FirstName = _firstName,
                        TransId = _transId,
                        TransDate = DateTimeHelper.NowDateTimeOffset.ToString("MM/dd/yyyy hh:mm:ss"),
                        Amount = _amount
                    };
                default:
                    return null;
            }
        }

        private string GetShortedLanguageCode(string languageCode)
        {
            var shortedLanguageCode = _languageService.GetLanguageList()
                .FirstOrDefault(l => string.Equals(l.LanguageCode, languageCode, StringComparison.OrdinalIgnoreCase))?.ShortedLanguageCode ?? _appSetting.DefaultShortedLanguageCode;

            return shortedLanguageCode.ToUpperInvariant();
        }
    }
}
