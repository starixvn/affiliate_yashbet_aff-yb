using System;

namespace Affiliate.YB.Models
{
    public class CustWalletInfo
    {
        public int CustId { get; set; }

        public int? SiteId { get; set; }

        public decimal? Balance { get; set; }

        public DateTime? FirstDepositTime { get; set; }
    }
}
