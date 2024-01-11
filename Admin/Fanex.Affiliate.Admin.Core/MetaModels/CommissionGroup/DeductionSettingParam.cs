using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class DeductionSettingParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "Deduction Id not exists!")]
        public int DeductionId { get; set; }

        public string ProductIds { get; set; }

        [Range(Validation.Zero, Validation.MaxPercentage, ErrorMessage = "Deducted Percentage is incorrect!")]
        public decimal DeductedPercentage { get; set; }
    }
}
