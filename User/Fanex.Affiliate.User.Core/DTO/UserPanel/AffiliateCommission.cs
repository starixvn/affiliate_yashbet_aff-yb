using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{

    [Serializable]
    public class AffiliateCommission
    {
        public DateTime MonthDate { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal TurnOver { get; set; }
        public decimal GGR { get; set; }

        public decimal PlatformFee { get; set; }


        public decimal DepositFee { get; set; }

        public decimal WithdrawalFee { get; set; }

        public decimal Bonus { get; set; }

        public decimal Rebate { get; set; }

        public decimal CommissionRate { get; set; }

        public decimal Commission { get; set; }
        public decimal P2pCommission { get; set; }

        public decimal SubAffCommission { get; set; }

        public decimal SubCommRate { get; set; }

        public int CalculatedType { get; set; }

        public bool IsMultiGroupProduct { get; set; }

        [SpParam(Ignored = true)]
        public decimal TotalCommission => Commission + P2pCommission + SubAffCommission;
    }
}
