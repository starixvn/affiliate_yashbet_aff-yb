using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class InsertUserParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please input user name!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "Please choose department!")]
        public int GroupId { get; set; }
    }
}
