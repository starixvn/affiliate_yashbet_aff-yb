namespace Fanex.Affiliate.User.Core.Utilities
{
    using Fanex.MailDeliver.MailClient;
    using Fanex.MailDeliver.MailClient.API;
    using MailDeliver.Model;
    using RazorEngine;
    using RazorEngine.Templating;
    using System.IO;

    public class MailProcessor : IMailProcessor
    {
        private readonly IAppSetting _appSetting;
        public MailProcessor(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public string BuildTemplateContent(string path, string pathBackup, object model)
        {
            try
            {
                var templateContent = File.ReadAllText(path);
                return Engine.Razor.RunCompile(templateContent, path, model: model);
            }
            catch
            {
                var templateContentBackup = File.ReadAllText(pathBackup);
                return Engine.Razor.RunCompile(templateContentBackup, pathBackup, model: model);
            }
        }

        public void Send(string content, string mailTo, string subject, string languageCode)
        {
            var mailItem = new MailRecord
            {
                MailTo = mailTo,
                Subject = SiteFriendlyNameHelper.ReplaceB2C(subject),
                Body = SiteFriendlyNameHelper.ReplaceB2C(content),
                MailBcc = _appSetting.MailBcc,
                MailCc = _appSetting.MailCc
            };

            AppSettings.AppMailId = _appSetting.GetAppMailIdByLanguageCode(languageCode);
            AppSettings.AppMailAddress = _appSetting.AppMailAddress;

            using (var client = new MailDeliver())
            {
                client.SendMail(mailItem);
            }
        }
    }
}