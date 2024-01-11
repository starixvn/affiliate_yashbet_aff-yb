using System;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
