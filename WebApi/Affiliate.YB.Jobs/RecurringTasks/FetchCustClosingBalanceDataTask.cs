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
    public interface IFetchCustClosingBalanceDataTask : IFetchDataTask { }
    public class FetchCustClosingBalanceDataTask : FetchDataByBatchForMultiRepoTaskBase<CustClosingBalanceModel>, IFetchCustClosingBalanceDataTask
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;

        protected override int PagingSize => 2000;

        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        public FetchCustClosingBalanceDataTask(ILicenseeApi licenseeApi,
            IThirdPartyImportRepository _thirdPartyRepo,
            ILogger logger,
            ITrackerService timeTrackerService,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_CUST_CLOSING_BALANCE_SETTING), timeTrackerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _licenseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
        }

        public override async Task FetchDataAsync()
        {
            try
            {
                var queryValue = await GetLastTrackingValueAsync();
                var fromDate = Convert.ToDateTime(queryValue);
                var pageIndex = 0;
                while (true)
                {
                    var data = await GetDataAsync(fromDate, pageIndex, PagingSize);
                    await ImportDataByBatchAsync(data);
                    pageIndex++;
                    if (data.Count() < PagingSize)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

        }

        protected override async Task ImportDataByBatchAsync(IEnumerable<CustClosingBalanceModel> data)
        {
            if (!data.Any())
                return;

            var importData = data;
            await ImportDataToMultipleRepoAsync(importData);
            var trackValue = GetInsertTrackingValue(importData);
            await _timeTrackerService.SaveTrackerAsync(TableName,
                new TrackerInfo
                {
                    CalledTime = DateTime.Now,
                    KeyValue = trackValue
                });

        }

        protected override Task<IEnumerable<CustClosingBalanceModel>> GetDataAsync(string trackValue, int batchCount)
        {
            throw new NotImplementedException();
        }

        private Task<IEnumerable<CustClosingBalanceModel>> GetDataAsync(DateTime fromDate, int pageIndex, int pageSize)
            => _licenseeApi.GetCustClosingBalance(fromDate, pageIndex, pageSize);

        protected override string GetInsertTrackingValue(IEnumerable<CustClosingBalanceModel> data)
            => data.Max(d => d.Time).ToString(DateTimeFormat);

        protected override async Task<string> GetLastTrackingValueAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var date = string.IsNullOrWhiteSpace(tracker.KeyValue) ? new DateTime() : Convert.ToDateTime(tracker.KeyValue);
            var queryId = GetAdjustmentId(date);
            return queryId.ToString(DateTimeFormat);
        }

        private DateTime GetAdjustmentId(DateTime date)
            => date == new DateTime() ? date : date.AddMinutes(Settings.AdjustmentValue);
    }
}
