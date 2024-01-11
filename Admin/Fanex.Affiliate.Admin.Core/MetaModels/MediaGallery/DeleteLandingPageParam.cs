using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class DeleteLandingPageParam : UserInfoParam
    {
        [Required(ErrorMessage = "Please select the items to delete")]
        public string PageIds { get; set; }
    }
}
