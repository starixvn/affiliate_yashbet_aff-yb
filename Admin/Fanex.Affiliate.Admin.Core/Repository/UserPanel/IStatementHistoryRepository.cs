using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.UserPanel
{
    public interface IStatementHistoryRepository
    {
        EntityResult<SubAffiliate> GetSubAffiliate(SubAffiliateParam param);

        EntityResult<StatementHistory> GetStatementHistoryList(AffiliatePagingByDateParam param);

        DailyEarningResult GetDailyEarning(DailyEarningParam param);

        IEnumerable<DailyEarningDetail> GetDailyEarningDetail(DailyEarningParam param);
    }
}
