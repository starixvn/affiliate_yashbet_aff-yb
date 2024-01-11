using Affiliate.YB.Comparer;
using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Constants;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Models;
using Affiliate.YB.Params;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Product9W
{
    public interface IFetchTpWager9WDataTask : IFetchDataTask
    {
    }

    public class FetchTpWager9WDataTask : FetchDataByBatchForMultiRepoTaskBase<TpWager9W>, IFetchTpWager9WDataTask
    {
        private readonly IProduct9WApi _licenseeApi;
        private readonly ILogger _logger;

        public FetchTpWager9WDataTask(IProduct9WApi licenseeApi,
             IThirdPartyImportRepository _thirdPartyRepo,
             ILogger logger,
             ITrackerService timeTrackerService,
             IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_TPWAGER_9W_SETTING), timeTrackerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _licenseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
        }

        protected override async Task<IEnumerable<TpWager9W>> GetDataAsync(string trackValue, int batchCount)
        {
            var fromDate = DateTime.Parse(trackValue);
            var toDate = DateTime.Now;
            return await _licenseeApi.GetTpWagerAsync(new BaseParam {
                FromDate = fromDate,
                ToDate  = toDate,
                PageIndex = 0,
                PageSize = batchCount
            });
        }

        protected override string GetInsertTrackingValue(IEnumerable<TpWager9W> data)
            => data.Max(d => d.UpdateTime).Value.ToString(Format.DateTimeFormat);

        protected override async Task<string> GetLastTrackingValueAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.KeyValue, new StringToDateTimeComparer());
            var date = string.IsNullOrWhiteSpace(tracker.KeyValue) 
                ? throw new ArgumentException($"Not valid DateTime Tracker Value {tracker.KeyValue} of table {TableName}") 
                : DateTime.Parse(tracker.KeyValue);
            var adjustDate = GetAdjustmentDate(date);
            return adjustDate.ToString(Format.DateTimeFormat);
        }

        private DateTime GetAdjustmentDate(DateTime date)
            => date.AddMinutes(Settings.AdjustmentValue);
    }
}
