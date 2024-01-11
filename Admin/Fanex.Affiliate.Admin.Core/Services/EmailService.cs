using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.ViewModels;
using System;
using System.Linq;
using System.Web;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMailProcessor _mailProcessor;

        private readonly IAppSetting _appSetting;

        private readonly ILanguageService _languageService;

        public EmailService(IMailProcessor mailProcessor, IAppSetting appSetting, ILanguageService languageService)
        {
            _mailProcessor = mailProcessor;
            _appSetting = appSetting;
            _languageService = languageService;
        }

        public void SendApprovedEmail(AffInformationEmail emailParam)
        {
            _languageService.SetCulture(emailParam.Language);
            SendAffInforEmail(emailParam);
            _languageService.SetCulture(_appSetting.DefaultLanguageCode);
        }

        private void SendAffInforEmail(AffInformationEmail emailParam)
        {
            const string TemplateName = "AffiliateInformation";
            var language = emailParam.Language;
            var shortedLanguageCode = GetShortedLanguageCode(language);

            var model = new AffInformationEmail
            {
                MailContact = GetMailContactByLanguageCode(shortedLanguageCode),
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                UserSiteUrl = _appSetting.GetAffSiteUrlByShortedLanguageCode(shortedLanguageCode),
                FirstName = emailParam.FirstName,
                UserName = emailParam.UserName,
                Password = emailParam.Password,
                AffCode = emailParam.AffCode
            };

            var content = GetEmailTextContent(TemplateName, language, model);

            var contentToSend = emailParam.IsHtml ?
                GetEmailHtmlContent(content, Lang.EmailTitleAffInfor, language, emailParam.RootUrl) :
                content;

            _mailProcessor.Send(contentToSend, emailParam.MailTo, Lang.EmailSubjectAffInfor, shortedLanguageCode);
        }

        private string GetEmailTextContent(string templateName, string prefrerredLanguage, object model)
        {
            var viewPath = string.Format(TemplateString.EmailTemplatePath, templateName, prefrerredLanguage);
            var viewPathBackup = string.Format(TemplateString.EmailTemplatePath, templateName, _appSetting.DefaultLanguageCode);
            var physicalPath = HttpContext.Current.Server.MapPath(viewPath);
            var physicalPathBackup = HttpContext.Current.Server.MapPath(viewPathBackup);

            return _mailProcessor.BuildTemplateContent(physicalPath, physicalPathBackup, model);
        }

        private string GetEmailHtmlContent(string content, string title, string language, string domain)
        {
            var viewModel = new EmailTemplateViewModel
            {
                RootUrl = domain,
                UserSiteUrl = _appSetting.AsianAffSiteUrl,
                ImageRootUrl = string.Format(TemplateString.EmailImageRootPath, domain),
                Language = language,
                Content = content,
                Title = title
            };
            var viewPath = HttpContext.Current.Server.MapPath("~/Areas/Portal/Views/Shared/Email/CommonLayout.cshtml");
            var viewPathBackup = viewPath;

            return _mailProcessor.BuildTemplateContent(viewPath, viewPathBackup, viewModel);
        }

        public void SendPaymentRequestUpdateEmail(PaymentRequestUpdateEmail emailParam)
        {
            var language = emailParam.Language;
            _languageService.SetCulture(language);
            var shortedLanguageCode = GetShortedLanguageCode(language);
            emailParam.MailContact = GetMailContactByLanguageCode(shortedLanguageCode);
            emailParam.SiteFriendlyName = _appSetting.SiteFriendlyName;
            emailParam.DefaultCurrency = _appSetting.DefaultCurrency;

            var TemplateName = emailParam.IsApproved ? "PaymentRequest/Approved" : "PaymentRequest/Rejected";

            var content = GetEmailTextContent(TemplateName, language, emailParam);
            var subject = emailParam.IsApproved ?
                string.Format(Lang.EmailSubjectPaymentRequestApproved, emailParam.TransId) :
                string.Format(Lang.EmailSubjectPaymentRequestRejected, emailParam.TransId);
            var title = emailParam.IsApproved ? Lang.EmailTitlePaymentRequestApproved : Lang.EmailTitlePaymentRequestRejected;

            var contentToSend = emailParam.IsHtml ?
                GetEmailHtmlContent(content, title, language, emailParam.RootUrl) :
                content;

            _mailProcessor.Send(contentToSend, emailParam.MailTo, subject, shortedLanguageCode);
            _languageService.SetCulture(_appSetting.DefaultLanguageCode);
        }

        public void SendDisapprovedEmail(AffInformationEmail emailParam)
        {
            _languageService.SetCulture(emailParam.Language);
            SendDisapprovedAffiliateEmail(emailParam);
            _languageService.SetCulture(_appSetting.DefaultLanguageCode);
        }

        private void SendDisapprovedAffiliateEmail(AffInformationEmail emailParam)
        {
            const string TemplateName = "AffiliateDisapproved";
            var language = emailParam.Language;
            var shortedLanguageCode = GetShortedLanguageCode(language);

            var model = new AffInformationEmail
            {
                MailContact = GetMailContactByLanguageCode(shortedLanguageCode),
                SiteFriendlyName = _appSetting.SiteFriendlyName,
                FirstName = emailParam.FirstName
            };

            var content = GetEmailTextContent(TemplateName, language, model);

            var contentToSend = emailParam.IsHtml ?
                GetEmailHtmlContent(content, Lang.EmailTitleAffDisapproved, language, emailParam.RootUrl) :
                content;

            _mailProcessor.Send(contentToSend, emailParam.MailTo, Lang.EmailSubjectAffDisapproved, shortedLanguageCode);
        }

        private string GetMailContactByLanguageCode(string shortedLanguageCode)
        {
            return _appSetting.GetMailContactByShortedLanguageCode(shortedLanguageCode);
        }

        private string GetShortedLanguageCode(string languageCode)
        {
            var shortedLanguageCode = _languageService.GetLanguageList()
                .FirstOrDefault(l => string.Equals(l.LanguageCode, languageCode, StringComparison.OrdinalIgnoreCase))?.ShortedLanguageCode ?? _appSetting.DefaultShortedLanguageCode;

            return shortedLanguageCode.ToUpperInvariant();
        }
    }
}
