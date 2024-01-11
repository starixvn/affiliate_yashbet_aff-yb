using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class UpdatePolicyParam : UserInfoParam
    {
        [Required]
        public string GroupPolicies { get; set; }
    }
}
