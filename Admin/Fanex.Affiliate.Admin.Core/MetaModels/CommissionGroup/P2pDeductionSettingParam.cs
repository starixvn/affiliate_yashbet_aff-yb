using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class P2PDeductionSettingParam : TierDeductionSettingParam
    {
        [Range(Validation.Zero, Validation.MaxPercentage, ErrorMessage = "Fixed Rate Percentage is incorrect!")]
        public decimal Commission { get; set; }
    }
}
