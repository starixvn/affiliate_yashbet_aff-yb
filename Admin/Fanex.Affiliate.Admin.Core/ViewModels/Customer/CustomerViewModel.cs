using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public string AffName { get; set; }

        public IEnumerable<Language> LanguageList { get; set; }

        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<Currency> CurrencyList { get; set; }

        public IEnumerable<Currency> BaseCurrencyList { get; set; }

        public IEnumerable<AffTypeInfo> AffTypeList { get; set; }
    }
}
