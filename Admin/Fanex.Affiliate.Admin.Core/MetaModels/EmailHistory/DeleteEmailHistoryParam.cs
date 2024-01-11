using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory
{
    public class DeleteEmailHistoryParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please select emails to delete")]
        public string EmailIdList { get; set; }
    }
}
