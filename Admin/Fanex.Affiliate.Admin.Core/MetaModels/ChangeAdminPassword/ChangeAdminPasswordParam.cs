
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.ChangeAdminPassword
{
    public class ChangeAdminPasswordParam : UserInfoParam
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression("^(?=.*[0-9]+.*[0-9]+)(?=.*[a-zA-Z].*[a-zA-Z]).{6,}$", ErrorMessage = "Password must have at least 6 characters, including at least 2 letters and at least 2 digits.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
