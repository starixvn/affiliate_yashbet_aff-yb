using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels
{
    public class AffiliateBasicInfoParam : UserInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "AffId not exists!")]
        public int AffId { get; set; }
    }
}
