using System;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel.PlayerTransferBalance
{
    public class PlayerTransferBalanceHistoryInfo
    {
        public int TransId { get; set; }

        public DateTime TransTime { get; set; }

        public string PlayerName { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public decimal BasedAmount { get; set; }

        public decimal AffPlayerAmount { get; set; }

        public string Status { get; set; }
    }
}
