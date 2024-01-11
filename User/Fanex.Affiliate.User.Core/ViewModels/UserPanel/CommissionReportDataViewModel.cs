using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class CommissionReportDataViewModel
    {
        public IList<AffiliateCommisionByMonth> Data { get; set; }
        public AffiliateCommission Total { get; set; }
    }
}
