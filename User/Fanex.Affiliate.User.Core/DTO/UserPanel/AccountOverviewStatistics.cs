namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class AccountOverviewStatistics
    {
        public int NewSignups { get; set; }

        public int ActivePlayers { get; set; }

        public int QualifiedMembers { get; set; }

        public decimal Deposit { get; set; }

        public decimal Turnover { get; set; }

        public int TotalPlayers { get; set; }

        public decimal TurnoverUp { get; set; }

        public decimal DepositUp { get; set; }

        public decimal BalanceBroughtForward { get; set; }
    }
}