using Fanex.Affiliate.Admin.Core.Resources;
using System;
using System.Configuration;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public class AppSetting : IAppSetting
    {
        public static IAppSetting Instance { get; } = new AppSetting();

        public string InternalAffiliateUserSiteURL => GetValue("internalAffiliateUserSiteURL");
        public string AffUserSiteURL => GetValue("AffiliateUserSiteURL");

        public string Environment => GetValue("Environment");

        public bool IsProduction => Environment.Equals("PRODUCTION", StringComparison.OrdinalIgnoreCase);

        public string CcMail => GetValue("ccMail");

        public string BccMail => GetValue("bccMail");

        public string JsVersion => GetValue("JsVersion");

        public bool IsDev => Environment.Equals("DEV", StringComparison.OrdinalIgnoreCase);

        public string GetValue(string key) => ConfigurationManager.AppSettings[key];

        public string MailContact => GetValue("MailContact");

        public string AppMailId => GetValue("app_mail_id");

        public string AppMailAddress => GetValue("app_mail_address");

        public string SentryDsn => GetValue("SentryDsn");

        public string LoggingCategory => GetValue("LoggingCategory");

        public string RabbitMQConnectionString => GetValue("RabbitMQConnectionString");

        public string BannerRootUrl => GetValue("BannerUrl");

        public string[] BannerFolders => GetValue("BannerFolders").Split(';');

        public int ImageSize => Convert.ToInt32(GetValue("imagesize"));

        public string ServerId => GetValue("ServerID");

        public string SiteName => GetValue("SiteName");

        public string SiteFriendlyName => Lang.ResourceManager.GetString(GetValue("SiteFriendlyName"));

        public string DefaultLanguage => GetValue("DefaultLanguage");

        public string DefaultLanguageCode => GetValue("DefaultLanguageCode");

        public string DefaultShortedLanguageCode => GetValue("DefaultShortedLanguageCode");

        public string DefaultCurrency => GetValue("DefaultCurrency");

        public string MemberSiteApiUrl => GetValue("MemberSiteApiUrl");

        public int MemberSiteId => Convert.ToInt32(GetValue("MemberSiteId"));

        public string MemberSiteProxyUrl => GetValue("MemberSiteProxyUrl");

        public string MemberSiteApiUsername => GetValue("MemberSiteApiUsername");

        public string MemberSiteApiPassword => GetValue("MemberSiteApiPassword");

        public bool IsUseMemberSiteProxy => Convert.ToBoolean(GetValue("IsUseProxyToGetMemberSiteApi"));
    }
}
