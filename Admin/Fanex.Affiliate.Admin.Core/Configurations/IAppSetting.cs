using System.Diagnostics.CodeAnalysis;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public interface IAppSetting
    {
        string GetValue(string key);

        string MailContact { get; }

        string GetMailContactByShortedLanguageCode(string shortedLanguageCode);

        string AppMailId { get; }

        string AppMailAddress { get; }

        string GetAppMailIdByShortedLanguageCode(string shortedLanguageCode);

        string InternalEuropeanAffiliateUrl { get; }

        string InternalAsianAffiliateUrl { get; }

        string EuropeanAffSiteUrl { get; }

        string AsianAffSiteUrl { get; }

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

        string MemberSiteApiAuthenticationKey { get; }

        bool IsUseMemberSiteProxy { get; }

        string AffSiteUrl { get; }

        [SuppressMessage("Major Code Smell", "S3995:URI return values should not be strings", Justification = "Reviewed. Suppression is OK here.")]
        string GetAffSiteUrlByShortedLanguageCode(string shortedLanguageCode);
    }
}
