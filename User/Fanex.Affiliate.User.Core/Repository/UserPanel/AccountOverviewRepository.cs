using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class AccountOverviewRepository : IAccountOverviewRepository
    {
        private readonly IObjectDBBuilder _builder;

        public AccountOverviewRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public LastLoginInfo GetLastLoginInfo(int affId)
        {
            const string uSpKey = "Aff_User_LoginInfo_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.QueryEntity<LastLoginInfo>(new { affId });
            }
        }

        public IEnumerable<PlayerDetail> GetPlayerDetailList(PlayerDetailParam param)
        {
            const string uSpKey = "Aff_User_Overview_PlayersDetail_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.Query<PlayerDetail>(param);
            }
        }

        public IEnumerable<SubAffDetail> GetSubAffDetails(SubAffDetailParam param)
        {
            const string uSpKey = "Aff_User_Overview_SubAffiliate_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.Query<SubAffDetail>(param);
            }
        }

        public IEnumerable<OverviewDetail> GetUserOverviewDetailList(int affId, DateTime monthDate)
        {
            const string uSpKey = "Aff_User_Overview_Detail_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.Query<OverviewDetail>(new { affId, monthDate });
            }
        }

        public OverviewSummary GetUserOverviewSummary(int affId, DateTime monthDate)
        {
            const string uSpKey = "Aff_User_Overview_Summary_Sel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.QueryEntity<OverviewSummary>(new { affId, monthDate });
            }
        }
    }
}
