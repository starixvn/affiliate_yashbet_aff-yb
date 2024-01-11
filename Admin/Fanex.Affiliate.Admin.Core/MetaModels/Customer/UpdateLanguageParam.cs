using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Customer
{
    public class UpdateLanguageParam : AffiliateBasicInfoParam
    {
        [Required]
        public string PreferredLanguage { get; set; }
    }
}
