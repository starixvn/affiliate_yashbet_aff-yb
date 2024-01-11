using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using System.Web;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class EmailService : IEmailService
    {
        private readonly IMailProcessor _mailProcessor;
        private readonly ILanguageService _languageService;
        private readonly IAppSetting _appSetting;

        public EmailService(IMailProcessor mailProcessor, ILanguageService languageService, IAppSetting appSetting)
        {
            _mailProcessor = mailProcessor;
            _languageService = languageService;
            _appSetting = appSetting;
        }

        private string SetLanguage(string language)
        {
            var currentLanguage = _languageService.GetCurrentLanguage();

            _languageService.SetCulture(language);

            return currentLanguage;
        }

        private void ResetLanguage(string language)
        {
            _languageService.SetCulture(language);
        }

        public void SendRecoverPasswordEmail(RecoverPasswordEmailParam emailParam)
        {
            var preferredLanguage = emailParam.User.PreferredLanguage;
            var prevLanguage = SetLanguage(preferredLanguage);

            const string TemplateName = "RecoverPassword";
            emailParam.SiteFriendlyName = _appSetting.SiteFriendlyName;
            var emailContent = GetEmailTextContent(TemplateName, preferredLanguage, emailParam);
            if (!emailParam.User.MailOption)
            {
                emailContent = GetEmailHtmlContent(emailContent, Lang.EmailTitlePasswordRecovery, preferredLanguage, emailParam.RootDomain);
            }

            _mailProcessor.Send(emailContent, emailParam.MailTo, Lang.SubjectForgetPass, preferredLanguage);

            ResetLanguage(prevLanguage);
        }

        public void SendNewPasswordEmail(NewPasswordEmailParam emailParam)
        {
            var preferredLanguage = emailParam.User.PreferredLanguage;
            var prevLanguage = SetLanguage(preferredLanguage);

            const string TemplateName = "NewPassword";
            emailParam.SiteFriendlyName = _appSetting.SiteFriendlyName;
            var emailContent = GetEmailTextContent(TemplateName, preferredLanguage, emailParam);
            if (!emailParam.User.MailOption)
            {
                emailContent = GetEmailHtmlContent(emailContent, Lang.EmailTitleNewPassword, preferredLanguage, emailParam.RootDomain);
            }

            _mailProcessor.Send(emailContent, emailParam.MailTo, Lang.SubjectNewPass, preferredLanguage);

            ResetLanguage(prevLanguage);
        }

        public void SendPaymentEmail(PaymentEmailParam emailParam)
        {
            var preferredLanguage = emailParam.User.PreferredLanguage;
            var prevLanguage = SetLanguage(preferredLanguage);
            var isCancel = emailParam.PaymentType == PaymentRequestType.Cancel;

            var templateName = isCancel ? "PaymentCancel" : "PaymentRequest";
            var subject = isCancel ? string.Format(Lang.PaymentRequestCancelSubject, emailParam.TransId) : string.Format(Lang.PaymentRequestSubject, emailParam.TransId);
            emailParam.SiteFriendlyName = _appSetting.SiteFriendlyName;
            var emailContent = GetEmailTextContent(templateName, preferredLanguage, emailParam);

            if (!emailParam.User.MailOption)
            {
                emailContent = GetEmailHtmlContent(emailContent, isCancel ? Lang.EmailTitlePaymentCancel : Lang.EmailTitlePayment, preferredLanguage, emailParam.RootDomain);
            }

            _mailProcessor.Send(emailContent, emailParam.MailTo, subject, preferredLanguage);

            ResetLanguage(prevLanguage);
        }

        public void SendNoticeEmailToCsd(NoticeEmailParam emailParam)
        {
            var preferredLanguage = emailParam.User.PreferredLanguage;
            var prevLanguage = SetLanguage(SystemLanguage.EnglishLanguageCode);

            const string TemplateName = "Csd";
            emailParam.SiteFriendlyName = _appSetting.SiteFriendlyName;
            var emailContent = GetEmailTextContent(TemplateName, SystemLanguage.EnglishLanguageCode, emailParam);

            var subject = string.Format(Lang.CsdSubject, emailParam.UserName);
            _mailProcessor.Send(emailContent, emailParam.MailTo, subject, preferredLanguage);

            ResetLanguage(prevLanguage);
        }

        public void SendVerificationEmail(VerificationAccountEmailParam emailParam)
        {
            var preferredLanguage = emailParam.User.PreferredLanguage;
            var prevLanguage = SetLanguage(preferredLanguage);

            const string TemplateName = "VerificationAccount";
            var emailContent = GetEmailTextContent(TemplateName, preferredLanguage, emailParam);
            if (!emailParam.User.MailOption)
            {
                emailContent = GetEmailHtmlContent(emailContent, Lang.EmailTittleVerificationAccount, preferredLanguage, emailParam.RootDomain);
            }

            _mailProcessor.Send(emailContent, emailParam.MailTo, Lang.SubjectVerificationAccount, preferredLanguage);

            ResetLanguage(prevLanguage);
        }

        private string GetEmailTextContent(string templateName, string prefrerredLanguage, object model)
        {
            var viewPath = string.Format(TemplateString.EmailTemplatePath, templateName, prefrerredLanguage);
            var viewPathBackup = string.Format(TemplateString.EmailTemplatePath, templateName, _appSetting.DefaultLanguageCode);
            var physicalPath = HttpContext.Current.Server.MapPath(viewPath);
            var physicalPathBackup = HttpContext.Current.Server.MapPath(viewPathBackup);

            var encodedContent = _mailProcessor.BuildTemplateContent(physicalPath, physicalPathBackup, model);
            var decodedContent = HttpUtility.HtmlDecode(encodedContent);
            return decodedContent;
        }

        private string GetEmailHtmlContent(string content, string title, string language, string domain)
        {
            var viewModel = new EmailTemplateViewModel
            {
                RootUrl = domain,
                ImageRootUrl = string.Format(TemplateString.EmailImageRootPath, domain),
                Language = language,
                Content = content,
                Title = title
            };
            var viewPath = HttpContext.Current.Server.MapPath("~/Views/Shared/Email/CommonLayout.cshtml");
            var viewPathBackup = viewPath;

            var encodedContent = _mailProcessor.BuildTemplateContent(viewPath, viewPathBackup, viewModel);
            var decodedContent = HttpUtility.HtmlDecode(encodedContent);
            return decodedContent;
        }
    }
}