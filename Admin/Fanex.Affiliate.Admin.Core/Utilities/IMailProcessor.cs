namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public interface IMailProcessor
    {
        string BuildTemplateContent(string path, string pathBackup, object model);

        void Send(string content, string mailTo, string subject, string shortedLanguageCode);
    }
}
