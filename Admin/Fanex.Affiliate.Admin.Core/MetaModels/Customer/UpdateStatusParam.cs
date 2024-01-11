using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Customer
{
    public class UpdateStatusParam : AffiliateBasicInfoParam
    {
        [Required]
        public string Status { get; set; }
    }
}
