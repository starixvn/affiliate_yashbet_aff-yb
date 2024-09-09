using System;

namespace Fanex.Affiliate.Admin.Core.Extensions
{
    public static class StringExtension
    {
        public static bool MatchWith(this string value, string strToMatch)
        {
            return string.Equals(value, strToMatch, StringComparison.OrdinalIgnoreCase);
        }
    }
}
