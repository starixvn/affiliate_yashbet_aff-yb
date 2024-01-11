using Fanex.Affiliate.User.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class UserLoginParam
    {
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "LoginWrongInfo")]
        public string LoginInfo { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrmsgEntryValidCode")]
        public string ValidationKey { get; set; }

        public string LoginIp { get; set; }

        public string Language { get; set; }

        public string UserAgent { get; set; }

        public bool IsVerifyingAccount { get; set; }
    }
}