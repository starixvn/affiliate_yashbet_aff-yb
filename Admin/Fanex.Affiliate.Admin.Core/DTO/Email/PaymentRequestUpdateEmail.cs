namespace Fanex.Affiliate.Admin.Core.DTO.Email
{
    public class PaymentRequestUpdateEmail : EmailBase
    {
        public long TransId { get; set; }

        public string FirstName { get; set; }

        public decimal Amount { get; set; }

        public string TransDate { get; set; }

        public bool IsApproved { get; set; }

        public string DefaultCurrency { get; set; }
    }
}
