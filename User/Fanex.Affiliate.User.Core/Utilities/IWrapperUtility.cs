using System;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public interface IWrapperUtility
    {
        DateTime GetNowDateTime();

        Random GetRandomGenerator();
    }
}
