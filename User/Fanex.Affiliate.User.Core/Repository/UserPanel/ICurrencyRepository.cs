using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetBaseCurrencyList();
    }
}
