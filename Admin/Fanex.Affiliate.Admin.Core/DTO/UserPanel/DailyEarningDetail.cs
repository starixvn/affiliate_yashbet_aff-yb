namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    public class DailyEarningDetail
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string IbcUserName { get; set; }

        public decimal Ggr { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Bonus { get; set; }

        public decimal Rebate { get; set; }
    }
}
