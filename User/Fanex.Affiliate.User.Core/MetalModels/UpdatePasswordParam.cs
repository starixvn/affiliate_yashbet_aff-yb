using Fanex.Affiliate.User.Core.Resources;
using Fanex.Data;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class UpdatePasswordParam
    {
        public int AffId { get; set; }

        public int UserId => AffId;

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "OldPasswordisre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NewPasswordisre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ReenterNewPassw")]
        [DataType(DataType.Password)]
        [SpParam(Ignored = true)]
        public string ConfirmNewPassword { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
