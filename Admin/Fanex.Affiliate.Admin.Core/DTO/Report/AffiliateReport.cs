using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Report
{
    public class AffiliateReport
    {
        public int AffId { get; set; }

        public string UserName { get; set; }

        public string PaymentCurrency { get; set; }

        [JsonIgnore]
        public DateTime? MonthDate { get; set; }

        public string Description { get; set; }

        public string MonthDateString => MonthDate?.ToString("MM/yyyy") ?? string.Empty;

        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public int Signup { get; set; }

        public int Fdm { get; set; }

        public int ActiveMember { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Turnover { get; set; }

        public decimal Ggr { get; set; }

        public decimal Bonus { get; set; }

        public decimal Rebate { get; set; }

        public decimal PlatformFee { get; set; }

        public decimal DepositFee { get; set; }

        public decimal WithdrawalFee { get; set; }

        public decimal TotalFee { get; set; }

        public decimal CommissionRate { get; set; }

        public decimal Earning { get; set; }

        public decimal AffWithdrawal { get; set; }

        public decimal AffAdjustment { get; set; }

        public decimal BalanceBf { get; set; }

        public int NoOfSubAff { get; set; }

        public decimal SubAffEarning { get; set; }

        public decimal P2pEarning { get; set; }

        public decimal TotalEarning => Earning + SubAffEarning + P2pEarning;

        public bool IsMultiGroupProduct { get; set; }
    }
}
