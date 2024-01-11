using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class InsertUpdateLandingPageParam : UserInfoParam
    {
        public int PageId { get; set; }

        [Required(ErrorMessage = "Landing Page Name is required.")]
        [MaxLength(Validation.MaxLengthLandingPage, ErrorMessage = "Landing Page Name must not exceed 50 characters")]
        public string PageName { get; set; }

        [Required(ErrorMessage = "Landing Page Code is required.")]
        [MaxLength(Validation.MaxLengthLandingPage, ErrorMessage = "Landing Page Code must not exceed 50 characters")]
        public string PageCode { get; set; }
    }
}
