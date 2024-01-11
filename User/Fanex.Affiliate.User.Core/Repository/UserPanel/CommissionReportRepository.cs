using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class CommissionReportRepository : ICommissionReportRepository
    {
        private readonly IObjectDBBuilder _builder;

        public CommissionReportRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public IEnumerable<AffiliateCommission> GetAffiliateCommission(CommissionReportParam param)
        {
            const string uSpKey = "Aff_User_Rpt_Commission";
            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.Query<AffiliateCommission>(param);
            }
        }
    }
}
