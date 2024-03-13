
namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public interface IAppSetting
    {
        string GetValue(string key);

        string MailContact { get; }

        string AppMailId { get; }

        string AppMailAddress { get; }

        string InternalAffiliateUserSiteURL { get; }

        string AffUserSiteURL { get; }

        string Environment { get; }

        bool IsProduction { get; }

        bool IsDev { get; }

        string CcMail { get; }

        string BccMail { get; }

        string JsVersion { get; }

        string SentryDsn { get; }

        string LoggingCategory { get; }

        string RabbitMQConnectionString { get; }

        string BannerRootUrl { get; }

        string[] BannerFolders { get; }

        int ImageSize { get; }

        string ServerId { get; }

        string SiteName { get; }

        string SiteFriendlyName { get; }

        string DefaultLanguage { get; }

        string DefaultLanguageCode { get; }

        string DefaultShortedLanguageCode { get; }

        string DefaultCurrency { get; }

        string MemberSiteApiUrl { get; }

        int MemberSiteId { get; }

        string MemberSiteProxyUrl { get; }

        string MemberSiteApiUsername { get; }

        string MemberSiteApiPassword { get; }

        bool IsUseMemberSiteProxy { get; }
    }
}
