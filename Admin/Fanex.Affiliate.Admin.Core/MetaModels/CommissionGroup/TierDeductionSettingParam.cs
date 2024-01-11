
namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class TierDeductionSettingParam : DeductionSettingParam
    {
        public bool IsDeductedPercent { get; set; } = true;

        public string DeductedAmount { get; set; }

        public long GroupProductId { get; set; }
    }
}
