using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Domain
{
    public class DomainControlInfo
    {
        public int DomainId { get; set; }

        public int AffDomainId { get; set; }

        public string AffDomain { get; set; }

        public int RedirectDomainId { get; set; }

        public string RedirectDomain { get; set; }

        public string MarketId { get; set; }

        public string Market { get; set; }

        public bool IsUsed { get; set; }

        public string Creator { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateText => CreatedDate?.ToString("MM/dd/yyyy");
    }
}
