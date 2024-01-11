using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class DeleteTextParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please select texts to delete")]
        public string TextIdList { get; set; }

        public byte UserType { get; set; }
    }
}
