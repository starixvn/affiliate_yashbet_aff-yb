using Affiliate.YB.Models;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affiliate.YB.WebApi.Controllers
{
    public class BetReportController : ApiController
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetReportController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TransBetRecordInfo>> GetBetRecord(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = 2000)
        {
            var betlist = await dynamicRepository.FetchAsync<TransBetRecordInfo>(new GetBetRecordCriteria(fromDate, toDate, pageIndex, pageSize));
            return betlist;
        }

        [HttpGet]
        public async Task<IEnumerable<TpTransferInfo>> GetTpTransferInfo(int id, int totalRow)
            => await dynamicRepository.FetchAsync<TpTransferInfo>(new GetTpTransferInfoCriteria(id, totalRow));

        [HttpGet]
        public async Task<IEnumerable<DepositInfo>> GetDepositInfo(DateTime updatedTime, int id, int totalRow)
            => await dynamicRepository.FetchAsync<DepositInfo>(new GetDepositInfoCriteria(updatedTime, id, totalRow));

        [HttpGet]
        public async Task<IEnumerable<BonusDTO>> GetBonusInfo(int id, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoCriteria(id, batchCount));

        [HttpGet]
        public async Task<IEnumerable<BonusDTO>> GetBonusInfoByDate(DateTime calledTime, int pageIndex, int pageSize = 2000)
            => await dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoByDateCriteria(calledTime, pageIndex, pageSize));

        [HttpGet]
        public async Task<IEnumerable<Withdrawal>> GetWithdrawalInfo(DateTime updateTime, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<Withdrawal>(new GetWithdrawalInfoCriteria(updateTime, batchCount));

        [HttpGet]
        public async Task<IEnumerable<AdjustmentInfo>> GetAdjustmentInfos(int id, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<AdjustmentInfo>(new GetAdjustmentInfoCriteria(id, batchCount));
    }
}