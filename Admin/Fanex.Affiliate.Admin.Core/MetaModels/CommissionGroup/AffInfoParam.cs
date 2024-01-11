using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class AffInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "AffId not exists!")]
        public int AffId { get; set; }

        public int GroupId { get; set; }

        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Range(Validation.MinAffType, Validation.MaxAffType, ErrorMessage = "AffType is incorrect!")]
        public int AffType { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        public bool MailOption { get; set; }

        [Required(ErrorMessage = "Preferred Language is required!")]
        public string PreferredLanguage { get; set; }
    }
}
