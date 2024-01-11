namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class SiteFriendlyNameHelper
    {
        private readonly static string SiteFriendlyName = AppSetting.Instance.SiteFriendlyName;

        public static string ReplaceB2C(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text.Replace("B2C", SiteFriendlyName);
        }

        public static string ReplaceTranslateVariable(string orginalText, string varName, string text)
        {
            if (string.IsNullOrWhiteSpace(varName))
            {
                return varName;
            }

            return orginalText.Replace($"{{{varName}}}", text);
        }
    }
}