using System.Text.RegularExpressions;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class Parser
    {
        public static string GetDeviceInfo(string userAgent)
        {
            const string Pattern = @"(\S*\s*\(([^(]*)\))";
            var match = Regex.Match(userAgent, Pattern, RegexOptions.IgnoreCase);

            return match.Success ? match.Value : "Other";
        }
    }
}
