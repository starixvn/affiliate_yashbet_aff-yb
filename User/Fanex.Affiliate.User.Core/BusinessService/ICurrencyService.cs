using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ICurrencyService
    {
        IEnumerable<Currency> GetBaseCurrencyList();
    }
}
