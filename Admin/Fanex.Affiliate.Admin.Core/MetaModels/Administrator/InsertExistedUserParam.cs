using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class InsertExistedUserParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please input user name!")]
        public string UserName { get; set; }

        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "Please choose department!")]
        public int GroupId { get; set; }
    }
}
