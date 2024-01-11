using Fanex.Affiliate.User.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class VerificationAccountParam
    {
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "LoginWrongInfo")]
        public string LoginInfo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrmsgEntryVerificationCode")]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrmsgEntryCaptcha")]
        public string Captcha { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrmsgEntryVerificationCode")]
        public string HashMessage { get; set; }
    }
}
