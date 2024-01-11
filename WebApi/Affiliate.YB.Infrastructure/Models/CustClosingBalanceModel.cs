using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.YB.Models
{
    public class CustClosingBalanceModel
    {
        public DateTime Time { get; set; }

        public int CustId { get; set; }

        public int SiteId { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
