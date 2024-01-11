using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.YB.Params
{
    public class AffiliateInfoParam
    {
        public int CustId { get; set; }

        public int SiteId { get; set; }

        public string AffiliateName { get; set; }

        public string ParentAffiliate { get; set; } = null;

        public string AffiliateCode { get; set; }

        public string UserName { get; set; }
    }
}
