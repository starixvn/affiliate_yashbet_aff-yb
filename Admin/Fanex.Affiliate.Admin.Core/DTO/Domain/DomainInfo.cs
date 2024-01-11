using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Domain
{
    public class DomainInfo
    {
        public int DomainId { get; set; }

        public string DomainName { get; set; }

        public bool IsSSL { get; set; }

        public string Creator { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateText => CreatedDate?.ToString("MM/dd/yyyy");
    }
}
