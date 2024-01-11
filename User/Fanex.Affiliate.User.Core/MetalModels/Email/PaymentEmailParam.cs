using Fanex.Affiliate.User.Core.Enum;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.Email
{
    public class PaymentEmailParam : EmailBaseParam
    {
        public long TransId { get; set; }

        public DateTime RequestDate { get; set; }

        public decimal Amount { get; set; }

        public PaymentRequestType PaymentType { get; set; }

        public string DefaultCurrency { get; set; }
    }
}
