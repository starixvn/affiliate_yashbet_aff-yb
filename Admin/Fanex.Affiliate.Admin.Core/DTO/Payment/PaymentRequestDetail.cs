using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Payment
{
    [Serializable]
    public class PaymentRequestDetail : PaymentRequest
    {
        public int AffId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PaymentMethod { get; set; }

        public string PlayerUsername { get; set; }

        public string AdminDesc { get; set; }

        public string AffDesc { get; set; }

        public new string TransDateText => TransDate.ToString("MM/dd/yyyy HH:mm:ss");

        public new string CompletedDateText => CompletedDate.ToString("MM/dd/yyyy HH:mm:ss");

        public decimal TransAmount { get; set; }

        public string TransCurrencyCode { get; set; }

        public decimal OriginalTransAmount { get; set; }
    }
}
