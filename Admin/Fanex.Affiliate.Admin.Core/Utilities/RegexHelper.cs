using System;
using System.Text.RegularExpressions;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public static class RegexHelper
    {
        private const int TimeOutSecond = 2;

        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(TimeOutSecond));
        }
    }
}
