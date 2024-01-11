using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Affiliate
{
    public class PasswordParam : AffiliateBasicInfoParam
    {
        [Required]
        [RegularExpression("^(?=.*[0-9]+.*[0-9]+)(?=.*[a-zA-Z].*[a-zA-Z]).{6,}$")]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
