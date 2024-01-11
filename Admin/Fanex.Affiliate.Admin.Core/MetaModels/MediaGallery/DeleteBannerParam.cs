using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class DeleteBannerParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please select banners to delete")]
        public string BannerIdList { get; set; }

        public byte UserType { get; set; }
    }
}
