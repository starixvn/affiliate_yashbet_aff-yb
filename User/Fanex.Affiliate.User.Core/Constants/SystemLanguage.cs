using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Constants
{
    public static class SystemLanguage
    {
        private static readonly IAppSetting _appSetting = AppSetting.Instance;

        private static readonly IDictionary<string, string> _supportedLanguageForGoogleAnalytics = new Dictionary<string, string>
        {
            ["en-US"] = "en",
            ["zh-CN"] = "cs",
            ["th-TH"] = "th",
            ["vi-VN"] = "vn",
            ["id-ID"] = "id",
            ["hi-IN"] = "hi",
            ["ms-MY"] = "ms",
            ["km-KH"] = "kh",
            ["bn-BD"] = "bn"
        };

        public static IDictionary<string, string> SupportedLanguage => _supportedLanguageForGoogleAnalytics;

        public static string DefaultLanguageCode
        {
            get { return _appSetting.DefaultLanguageCode; }
        }

        public static string DefaultShortedLanguageCode
        {
            get { return _appSetting.DefaultShortedLanguageCode; }
        }

        public const string EnglishLanguageCode = "en-US";

        public const string IndonesiaLanguageCode = "id-ID";

        public static string GetUppercaseShortedCode(string languageCode)
        {
            if (_supportedLanguageForGoogleAnalytics.TryGetValue(languageCode, out string result) && !string.IsNullOrEmpty(result))
            {
                return result.ToUpperInvariant();
            }

            return DefaultShortedLanguageCode.ToUpperInvariant();
        }

        public static string GetLanguageText(string value)
        {
            return Lang.ResourceManager.GetString(value.Replace(" ", "")) ?? value;
        }
    }
}