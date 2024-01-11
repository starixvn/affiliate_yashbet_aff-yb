using Affiliate.YB.Models;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affiliate.YB.WebApi.Controllers
{
    public class LogReportController : ApiController
    {
        public IDynamicRepository Repo { get; }

        public LogReportController(IDynamicRepository repo)
        {
            Repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<RebateMLogModel>> GetRebateMLogs(DateTime fromDate, DateTime toDate)
            => await Repo.FetchAsync<RebateMLogModel>(new GetRebateMLogCriteria(fromDate, toDate));

        [HttpGet]
        public async Task<IEnumerable<DrawLogInfo>> GetDrawLogs(int id, int batchCount = 2000)
            => await Repo.FetchAsync<DrawLogInfo>(new GetDrawLogCritearia(id, batchCount));

        [HttpGet]
        public async Task<IEnumerable<EventDrawInfo>> GetEventDraws(int id, int batchCount = 2000)
            => await Repo.FetchAsync<EventDrawInfo>(new GetEventDrawCritearia(id, batchCount));


        [HttpGet]
        public async Task<IEnumerable<LogAdminAjustWalletInfo>> GetLogAdminAdjustWallet(int id, int batchCount = 2000)
            => await Repo.FetchAsync<LogAdminAjustWalletInfo>(new GetLogAdminAjustWalletCriteria(id, batchCount));

        [HttpGet]
        public async Task<IEnumerable<ReferralMLogModel>> GetReferralMLogs(int id, int batchCount = 2000)
            => await Repo.FetchAsync<ReferralMLogModel>(new GetReferralMLogCriteria(id, batchCount));

        [HttpGet]
        public async Task<IEnumerable<ReferralLogModel>> GetReferralLogs(int id, int batchCount = 2000)
            => await Repo.FetchAsync<ReferralLogModel>(new GetReferralLogCriteria(id, batchCount));
    }
}
