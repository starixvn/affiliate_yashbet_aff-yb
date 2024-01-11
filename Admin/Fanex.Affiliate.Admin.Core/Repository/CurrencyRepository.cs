using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public class CurrencyRepository : BaseRepository, ICurrencyRepository
    {
        public IEnumerable<Currency> GetBaseCurrencies()
        {
            const string spKey = "Aff_BaseCurrency_Sel";

            using (var objectDb = GetObjectDb(spKey))
            {
                return objectDb.Query<Currency>();
            }
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            const string spKey = "Aff_Currency_Sel";

            using (var objectDb = GetObjectDb(spKey))
            {
                return objectDb.Query<Currency>();
            }
        }

        public IEnumerable<Currency> GetPaymentCurrencies()
        {
            const string spKey = "Aff_PaymentCurrency_Sel";

            using (var objectDb = GetObjectDb(spKey))
            {
                return objectDb.Query<Currency>();
            }
        }
    }
}
