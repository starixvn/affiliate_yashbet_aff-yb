using System;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset NowDateTimeOffset => DateTimeOffset.Now;

        public static DateTime NowDateTime => DateTimeOffset.Now.DateTime;

        public static int CurrentYear => DateTimeOffset.Now.Year;

        public static DateTimeOffset UtcNowDateTimeOffset => DateTimeOffset.UtcNow;

        public static DateTime CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
            => new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero).DateTime;
    }
}