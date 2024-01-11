using System;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    public class DailyEarning
    {
        public DateTime? WinlostDate { get; set; }

        public decimal MemberGgr { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Bonus { get; set; }

        public decimal Rebate { get; set; }

        public string Date => WinlostDate.HasValue ? WinlostDate.Value.ToString("dd MMMM yyyy") : string.Empty;
    }
}
