using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class TierSettingParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "TierId not exists!")]
        public int TierId { get; set; }

        [Range(0, (double)Validation.InfinityAmount, ErrorMessage = "Min Amount not valid!")]
        public decimal MinAmount { get; set; }

        [Range(0, Validation.InfinityQualifiedMember, ErrorMessage = "Min Actived Players not valid!")]
        public int MinAp { get; set; }

        [Range(Validation.Zero, Validation.MaxPercentage, ErrorMessage = "Commission is incorrect!")]
        public decimal Commission { get; set; }

        public long GroupProductId { get; set; }
    }
}
