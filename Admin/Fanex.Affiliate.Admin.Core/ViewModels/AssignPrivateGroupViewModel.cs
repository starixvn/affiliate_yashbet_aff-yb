using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels
{
    public class AssignPrivateGroupViewModel
    {
        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<DeductionItem> DeductionList { get; set; }

        public AffiliateBasicInfo AffInfo { get; set; }

        public IEnumerable<Product> P2pProductList { get; set; }
    }
}
