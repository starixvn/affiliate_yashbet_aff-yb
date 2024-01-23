using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public class AppSetting : IAppSetting
    {
        public static IAppSetting Instance { get; } = new AppSetting();

        public string Environment => GetValue("Environment");

        public bool IsDev => Environment.Equals("Dev", StringComparison.OrdinalIgnoreCase);

        public bool IsLocal => Environment.Equals("Local", StringComparison.OrdinalIgnoreCase);

        public bool IsProduction => Environment.Equals("Production", StringComparison.OrdinalIgnoreCase);

        public int ServerId => Convert.ToInt32(GetValue("ServerId"));

        public IEnumerable<string> AllowedAffiliateTypes => GetValue("AllowedAffType").Split(',');

        public string JsVersion => GetValue("JsVersion");

        public string PromotionYBLink => GetValue("PromotionYBLink");

        public bool IsEuroSite => AllowedAffiliateTypes.Any(t => t == AffiliateType.European.ToString("D"));

        public string TextLinkTemplate => GetValue("TextLinkTemplate");

        public string BannerDomain => GetValue("BannerDomain");

        public string MailContact => GetValue("MailContact");

        public string GetMailContactByLanguageCode(string languageCode) => GetValue("MailContact" + SystemLanguage.GetUppercaseShortedCode(languageCode)) ?? MailContact;

        public string AppMailId => GetValue("app_mail_id");

        public string AppMailAddress => GetValue("app_mail_address");

        public string GetAppMailIdByLanguageCode(string languageCode) => GetValue("app_mail_id_" + SystemLanguage.GetUppercaseShortedCode(languageCode)) ?? AppMailId;

        public string MailCc => GetValue("MailCc");

        public string MailBcc => GetValue("MailBcc");

        public DateTime StatementAvailableFrom => Convert.ToDateTime(GetValue("StatementAvailableFrom"));

        public string RsaPublicKey => GetValue("RsaPublicKey");

        public string RsaPrivateKey => GetValue("RsaPrivateKey");

        public int LoginTimeout => Convert.ToInt32(GetValue("LoginTimeout"));

        public string SymmetricPassword => GetValue("SymmetricPassword");

        public string SymmetricSalt => GetValue("SymmetricSalt");

        public string GetValue(string key) => ConfigurationManager.AppSettings[key];

        public string SiteName => GetValue("SiteName");

        public string SiteFriendlyName => Lang.ResourceManager.GetString(GetValue("SiteFriendlyName"));

        public string MinWithdrawal => GetValue("MinWithdrawal");

        public string SubAffTextLinkTemplate => GetValue("SubAffTextLinkTemplate");

        public string DefaultLanguageCode => GetValue("DefaultLanguageCode");

        public string DefaultShortedLanguageCode => GetValue("DefaultShortedLanguageCode");

        public string DefaultCurrency => GetValue("DefaultCurrency");

        public string MaxCommission => GetValue("MaxCommission");

        public string MemberSiteApiUrl => GetValue("MemberSiteApiUrl");

        public string AccountSiteApiUrl => GetValue("AccountSiteApiUrl");

        public int MemberSiteId => Convert.ToInt32(GetValue("MemberSiteId"));

        public string MemberSiteProxyUrl => GetValue("MemberSiteProxyUrl");

        public string MemberSiteApiAuthenticationKey => GetValue("MemberSiteApiAuthenticationKey");

        public bool IsUseMemberSiteProxy => Convert.ToBoolean(GetValue("IsUseProxyToGetMemberSiteApi"));

        public string CurrencySymbol => GetValue("CurrencySymbol");

        public string CurrencyLabel => GetValue("CurrencyLabel");

        public string EndDatePromotion => GetValue("EndDatePromotion");

        public int RedNumberRGB => Convert.ToInt32(GetValue("RedNumberRGB"));

        public int GreenNumberRGB => Convert.ToInt32(GetValue("GreenNumberRGB"));

        public int BlueNumberRGB => Convert.ToInt32(GetValue("BlueNumberRGB"));

    }
}