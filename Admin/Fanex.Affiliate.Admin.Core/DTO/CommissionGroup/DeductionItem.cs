namespace Fanex.Affiliate.Admin.Core.DTO.CommissionGroup
{
    public class DeductionItem
    {
        public int DeductionId { get; set; }

        public string DeductionName { get; set; }

        public decimal DeductedPercentage { get; set; }

        public bool IsDeductedPercent { get; set; }

        public string DeductedAmount { get; set; }

        public decimal Commission { get; set; }

        public int GroupProductId { get; set; }
    }
}
