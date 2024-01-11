using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Report
{
    public class ReportCommonViewModel
    {
        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<Currency> BaseCurrencyList { get; set; }
    }
}
