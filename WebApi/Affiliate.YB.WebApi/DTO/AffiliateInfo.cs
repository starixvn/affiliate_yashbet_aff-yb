using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Affiliate.YB.WebApi.DTO
{
    public class AffiliateInfo
    {
        public int CustId { get; set; }

        public int SiteId { get; set; }

        public string AffiliateName { get; set; }

        public string ParentAffiliate { get; set; } = null;

        public string AffiliateCode { get; set; }

        public string UserName { get; set; }
    }
}