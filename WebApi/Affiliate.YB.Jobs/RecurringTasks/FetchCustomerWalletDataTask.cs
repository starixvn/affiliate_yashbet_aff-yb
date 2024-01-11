using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Constants;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Affiliate.YB.Utitlities;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks
{
    public interface IFetchCustomerWalletDataTask : IFetchDataTask
    { }

    public class FetchCustomerWalletDataTask : FetchDataForMultipleRepositoryTaskBase<CustWalletInfo>, IFetchCustomerWalletDataTask
    {
        private readonly ITrackerService _trackerService;
        private readonly ILicenseeApi _liceseeApi;
        private readonly ILogger _logger;

        public FetchCustomerWalletDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService trackerService
            , IOptionsSnapshot<FetchServiceSetting> setting)
            : base(thirdPartyImportRepository, setting.Get(ServiceConfigName.PULL_CUST_WALLET_SETTING))
        {
            _trackerService = trackerService;
            _liceseeApi = licenseeApi;
            _logger = logger;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await _trackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var fromDate = DateTime.Parse(tracker.KeyValue);
            await FetchDataAsync(fromDate);
        }

        public async Task FetchDataAsync(DateTime fromDate)
        {
            var toDate = GetAdjustmentTime();
            var data = await _liceseeApi.GetCustomerWalletAsync(fromDate);
            await ImportDataToMultipleRepoAsync(data);
            var tracker = new TrackerInfo { CalledTime = DateTime.Now, KeyValue = toDate.ToString() };
            await _trackerService.SaveTrackerAsync(TableName, tracker);
        }

        private DateTime GetAdjustmentTime()
            => DateTime.Now.AddMinutes(Settings.AdjustmentValue);

        protected override string BuildXml(IEnumerable<CustWalletInfo> data)
            => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}