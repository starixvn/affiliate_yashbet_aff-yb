using System;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public static class DateTimeHelper
    {
        public static bool IsValidDateRange(DateTime from, DateTime to, int monthRange = 6)
        {
            var maxMonth = from.AddMonths(monthRange);

            return DateTime.Compare(maxMonth, to) >= 0;
        }

        public static bool IsValidDateTime(DateTime from, DateTime to)
        {
            return DateTime.Compare(from, to) <= 0;
        }

        public static DateTimeOffset NowDateTimeOffset => DateTimeOffset.Now;

        public static DateTimeOffset UtcNowDateTimeOffset => DateTimeOffset.UtcNow;
    }
}
