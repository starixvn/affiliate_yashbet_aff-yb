namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class PlayerTransferBalanceInsParam
    {
        public int AffId { get; set; }

        public int PaymentAccountId { get; set; }

        public int PaymentAccountCurrencyId { get; set; }

        public int PlayerId { get; set; }

        public int PlayerCurrencyId { get; set; }

        public decimal Amount { get; set; }

        public decimal PaymentAccountExRate { get; set; }

        public decimal PlayerExRate { get; set; }
    }
}
