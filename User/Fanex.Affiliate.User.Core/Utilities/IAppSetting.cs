using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public interface IAppSetting
    {
        string GetValue(string key);

        string MailContact { get; }

        string GetMailContactByLanguageCode(string languageCode);

        string AppMailId { get; }

        string AppMailAddress { get; }

        string GetAppMailIdByLanguageCode(string languageCode);

        string Environment { get; }

        bool IsDev { get; }

        bool IsLocal { get; }

        bool IsProduction { get; }

        IEnumerable<string> AllowedAffiliateTypes { get; }

        int ServerId { get; }

        string JsVersion { get; }

        string PromotionYBLink { get; }

        bool IsEuroSite { get; }

        string TextLinkTemplate { get; }

        string BannerDomain { get; }

        string MailCc { get; }

        string MailBcc { get; }

        string RsaPublicKey { get; }

        string RsaPrivateKey { get; }

        int LoginTimeout { get; }

        DateTime StatementAvailableFrom { get; }

        string SymmetricPassword { get; }

        string SymmetricSalt { get; }

        string SiteFriendlyName { get; }

        string SiteName { get; }

        string MinWithdrawal { get; }

        string SubAffTextLinkTemplate { get; }

        string DefaultLanguageCode { get; }

        string DefaultShortedLanguageCode { get; }

        string DefaultCurrency { get; }

        string MaxCommission { get; }

        string MemberSiteApiUrl { get; }

        string AccountSiteApiUrl { get; }

        int MemberSiteId { get; }

        string MemberSiteProxyUrl { get; }

        string MemberSiteApiAuthenticationKey { get; }

        bool IsUseMemberSiteProxy { get; }

        string CurrencySymbol { get; }

        string CurrencyLabel { get; }

        string EndDatePromotion { get; }

        int RedNumberRGB { get; }

        int GreenNumberRGB { get; }

        int BlueNumberRGB { get; }
    }
}
