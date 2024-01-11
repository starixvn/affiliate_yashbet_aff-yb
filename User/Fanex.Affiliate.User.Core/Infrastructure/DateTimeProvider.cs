using Fanex.Affiliate.User.Core.Utilities;
using System;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTimeHelper.NowDateTime;
    }
}
