using Fanex.Affiliate.User.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class PaymentViewModel
    {
        public IEnumerable<Payment> PaymentList { get; set; }

        public Payment PendingPayment { get; set; }

        public string PreferredPayment { get; set; }

        public decimal BalBroughtForward { get; set; }

        public decimal MinAmount { get; set; }

        public bool CanRequest { get; set; } = true;

        public bool IsValidDate { get; set; }

        public bool IsValidAmount => BalBroughtForward > 0 && BalBroughtForward >= MinAmount;
    }
}
