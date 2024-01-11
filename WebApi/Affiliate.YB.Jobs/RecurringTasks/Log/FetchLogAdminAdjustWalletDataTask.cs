using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Log
{
    public interface IFetchLogAdminAdjustWalletDataTask : IFetchDataTask { }

    public class FetchLogAdminAdjustWalletDataTask : FetchDataByBatchForMultiRepoTaskBase<LogAdminAjustWalletInfo>, IFetchLogAdminAdjustWalletDataTask
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;

        public FetchLogAdminAdjustWalletDataTask(ILicenseeApi licenseeApi,
             IThirdPartyImportRepository _thirdPartyRepo,
             ILogger logger,
             ITrackerService timeTrackerService,
             IOptionsSnapshot<FetchServiceSetting> configuration)
             : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_LOG_ADMIN_ADJUST_WALLET_SETTING), timeTrackerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _licenseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
        }

        protected override Task<IEnumerable<LogAdminAjustWalletInfo>> GetDataAsync(string trackValue, int batchCount)
        {
            var id = Convert.ToInt32(trackValue);
            return _licenseeApi.GetLogAdminAdjustWallet(id, batchCount);
        }

        protected override string GetInsertTrackingValue(IEnumerable<LogAdminAjustWalletInfo> data)
            => data.Max(d => d.TransferId).ToString();

        protected override async Task<string> GetLastTrackingValueAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var id = string.IsNullOrWhiteSpace(tracker.KeyValue) ? 0 : int.Parse(tracker.KeyValue);
            var queryId = GetAdjustmentId(id);
            return queryId.ToString();
        }

        private int GetAdjustmentId(int id)
            => id - Settings.AdjustmentValue < 0 ? 0 : id - Settings.AdjustmentValue;
    }
}
