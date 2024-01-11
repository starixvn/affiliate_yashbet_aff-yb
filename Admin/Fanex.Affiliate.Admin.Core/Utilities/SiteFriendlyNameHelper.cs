using Fanex.Affiliate.Admin.Core.Configurations;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public static class SiteFriendlyNameHelper
    {
        public static string ReplaceB2C(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var siteFriendlyName = AppSetting.Instance.SiteFriendlyName;

            return text.Replace("B2C", siteFriendlyName);
        }
    }
}