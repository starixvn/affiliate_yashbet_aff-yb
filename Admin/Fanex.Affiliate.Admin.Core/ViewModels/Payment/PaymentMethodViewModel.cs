using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Payment
{
    public class PaymentMethodViewModel
    {
        public IEnumerable<KeyValuePair<string, string>> PaymentMethods { get; set; }

        public decimal MinAmount { get; set; }
    }
}
