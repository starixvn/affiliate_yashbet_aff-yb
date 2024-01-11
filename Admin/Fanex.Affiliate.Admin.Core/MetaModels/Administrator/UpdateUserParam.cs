using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class UpdateUserParam : UserInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "Account Id is invalid!")]
        public int AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "Please choose department!")]
        public int GroupId { get; set; }
    }
}
