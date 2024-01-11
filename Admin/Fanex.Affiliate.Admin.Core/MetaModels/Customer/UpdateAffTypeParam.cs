using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Customer
{
    public class UpdateAffTypeParam : AffiliateBasicInfoParam
    {
        [Range(Validation.MinAffType, Validation.MaxAffType, ErrorMessage = "AffType is incorrect!")]
        public int AffType { get; set; }
    }
}
