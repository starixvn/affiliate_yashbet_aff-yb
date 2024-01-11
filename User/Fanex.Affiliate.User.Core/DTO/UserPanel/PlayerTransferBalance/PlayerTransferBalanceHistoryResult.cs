using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel.PlayerTransferBalance
{
    public class PlayerTransferBalanceHistoryResult
    {
        public IEnumerable<PlayerTransferBalanceHistoryInfo> Items { get; set; }

        public int TotalRow { get; set; }

        public decimal TotalBaseAmount { get; set; }
    }
}
