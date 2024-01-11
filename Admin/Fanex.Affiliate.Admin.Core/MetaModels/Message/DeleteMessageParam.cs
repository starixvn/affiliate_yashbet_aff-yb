using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Message
{
    public class DeleteMessageParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please select messages to delete")]
        public string MessageIdList { get; set; }
    }
}
