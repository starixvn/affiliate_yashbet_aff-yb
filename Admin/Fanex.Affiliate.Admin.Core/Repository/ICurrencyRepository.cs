using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetBaseCurrencies();

        IEnumerable<Currency> GetCurrencies();

        IEnumerable<Currency> GetPaymentCurrencies();
    }
}
