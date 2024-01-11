
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Domain
{
    public class InsertUpdateDomainParam : UserInfoParam
    {
        public int DomainId { get; set; }

        [Required]
        public string DomainName { get; set; }

        public bool IsSSL { get; set; }

        public bool IsAffDomain { get; set; }
    }
}
