using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Jobs.Services;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Affiliate.YB.Utitlities;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Log
{
    public interface IFetchReferralLogDataTask : IFetchDataTask
    { }

    public class FetchReferralLogDataTask : FetchDataByBatchForMultiRepoTaskBase<ReferralLogModel>, IFetchReferralLogDataTask
    {
        private ILicenseeApi Api;
        private ILogger Logger;
        private ITrackerService TrackerService { get; }

        private const int API_SERVER_TIME_OFFSET = 0;

        public FetchReferralLogDataTask(ILicenseeApi licenseeApi,
             IThirdPartyImportRepository _thirdPartyRepo,
             ILogger logger,
             ITrackerService timeTrackerService,
             IOptionsSnapshot<FetchServiceSetting> configuration)
             : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_REFERRALLOG_SETTING), timeTrackerService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Api = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
            TrackerService = timeTrackerService;
        }

        protected override Task<IEnumerable<ReferralLogModel>> GetDataAsync(string trackValue, int batchCount)
        {
            var id = Convert.ToInt32(trackValue);
            return Api.GetReferralLogs(id, batchCount);
        }

        protected override string GetInsertTrackingValue(IEnumerable<ReferralLogModel> data)
            => data.Max(d => d.SQ).ToString();

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