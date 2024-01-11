using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.DTO.CommissionGroup
{
    public class CommissionGroupSetting : CommGroup
    {
        [Required]
        public IEnumerable<TierSetting> TierSettingList { get; set; }

        public IEnumerable<DeductionSetting> P2pDeductionList { get; set; }

        [Required]
        public IEnumerable<DeductionSetting> PlatformFeeList { get; set; }

        [Required]
        public DeductionSetting DepositFee { get; set; }

        [Required]
        public DeductionSetting WithdrawalFee { get; set; }
    }
}
