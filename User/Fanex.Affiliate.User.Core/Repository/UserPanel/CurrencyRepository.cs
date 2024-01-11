using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IObjectDBBuilder _builder;

        public CurrencyRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public IEnumerable<Currency> GetBaseCurrencyList()
        {
            const string uSpKey = "Aff_BaseCurrency_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.Query<Currency>();
            }
        }
    }
}
