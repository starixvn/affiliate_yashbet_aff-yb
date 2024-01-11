
using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Domain
{
    public class InsertUpdateDomainControlParam : UserInfoParam
    {
        public int DomainId { get; set; }

        [Required]
        public string MarketId { get; set; }

        [Range(Validation.MinId, Validation.MaxId)]
        public int AffDomainId { get; set; }

        [Range(Validation.MinId, Validation.MaxId)]
        public int RedirectDomainId { get; set; }

        public bool IsUsed { get; set; }
    }
}
