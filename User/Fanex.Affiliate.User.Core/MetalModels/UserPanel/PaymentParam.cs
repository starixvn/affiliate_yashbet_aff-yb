namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class PaymentParam : BaseParam
    {
        public int AffId { get; set; }

        public string PreferredPayment { get; set; }

        public decimal BalBroughtForward { get; set; }

        public decimal MinAmount { get; set; }

        public int Year { get; set; }
    }
}
