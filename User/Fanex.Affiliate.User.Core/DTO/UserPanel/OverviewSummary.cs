namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class OverviewSummary
    {
        public int Signups { get; set; }

        public int DepositedMembers { get; set; }

        public decimal BalanceBroughtForward { get; set; }

        public int ActivePlayers { get; set; }

        public int QualifiedActivePlayers { get; set; }

        public decimal Turnover { get; set; }

        public decimal Deposit { get; set; }

        public decimal Commission { get; set; }

        public int NoOfSubAff { get; set; }

        public decimal SubAffCommission { get; set; }

        public bool IsMaxSubLevel { get; set; }
    }
}
