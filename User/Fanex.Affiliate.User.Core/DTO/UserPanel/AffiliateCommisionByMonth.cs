using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class AffiliateCommisionByMonth
    {
        public int Month { get; set; }

        public string DiplayMonth { get; set; }

        public AffiliateCommission Data { get; set; }

        public IList<AffiliateCommission> ProductData { get; set; }

    }
}
