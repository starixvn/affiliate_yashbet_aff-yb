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

namespace Affiliate.YB.Jobs.RecurringTasks
{
    public interface IFetchWithdrawalInfoDataTask : IFetchDataTask
    { }

    public class FetchWithdrawalInfoDataTask : FetchDataByBatchForMultiRepoTaskBase<Withdrawal>, IFetchWithdrawalInfoDataTask
    {
        private ILicenseeApi Api { get; }
        private ILogger Logger { get; }

        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";
        private const int BatchCount = 2000;

        public FetchWithdrawalInfoDataTask(IThirdPartyImportRepository thirdPartyImportRepository,
            ITrackerService trackerService,
            ILogger logger,
            ILicenseeApi licenseeApi,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(thirdPartyImportRepository, configuration.Get(Constants.ServiceConfigName.PULL_WITHDRAWAL_INFO), trackerService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Api = licenseeApi;
        }

        public override async Task FetchDataAsync()
        {
            try
            {
                var queryValue = await GetLastTrackingValueAsync();
                var fromDate = Convert.ToDateTime(queryValue);

                var data = await GetDataAsync(fromDate, BatchCount);

                await ImportDataByBatchAsync(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        protected override async Task ImportDataByBatchAsync(IEnumerable<Withdrawal> data)
        {
            var importData = data;
            while (importData != null && importData.Any())
            {
                await ImportDataToMultipleRepoAsync(importData);
                var trackValue = GetInsertTrackingValue(importData);
                var fromTime = Convert.ToDateTime(trackValue);

                await _timeTrackerService.SaveTrackerAsync(TableName,
                    new TrackerInfo
                    {
                        CalledTime = DateTime.Now,
                        KeyValue = trackValue
                    });

                if (importData.Count() == BatchCount)
                {
                    importData = await GetDataAsync(fromTime, BatchCount);
                }
                else
                {
                    break;
                }
            }
        }

        protected override Task<IEnumerable<Withdrawal>> GetDataAsync(string trackValue, int batchCount)
        {
            throw new NotImplementedException();
        }

        private Task<IEnumerable<Withdrawal>> GetDataAsync(DateTime updateTime, int batchCount)
            => Api.GetWithdrawalInfo(updateTime, batchCount);

        protected override string GetInsertTrackingValue(IEnumerable<Withdrawal> data)
        {
            var trackValue = data.Max(d => d.UpdateTime);

            if (trackValue.HasValue)
            {
                return trackValue.Value.ToString();
            }
            throw new ArgumentException($"Can not insert nullable DateTime Tracker Value of table {TableName}");
        }

        protected override async Task<string> GetLastTrackingValueAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var date = string.IsNullOrWhiteSpace(tracker.KeyValue)
                ? throw new ArgumentException($"Not valid DateTime Tracker Value {tracker.KeyValue} of table {TableName}")
                : Convert.ToDateTime(tracker.KeyValue);
            var query = GetAdjustmentTime(date);
            return query.ToString(DateTimeFormat);
        }

        private DateTime GetAdjustmentTime(DateTime date) => date.AddMinutes(Settings.AdjustmentValue);
    }
}