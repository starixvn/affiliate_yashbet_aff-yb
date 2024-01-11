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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks
{
    public interface IFetchDepositInfoDataTask : IFetchDataTask { }

    public class FetchDepositInfoDataTask : FetchDataForMultipleRepositoryTaskBase<DepositInfo>, IFetchDepositInfoDataTask
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;
        private readonly ITrackerService _trackerService;

        private const int MAX_ROW_COUNT = 2000;

        public FetchDepositInfoDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService trackerService
            , IOptionsSnapshot<FetchServiceSetting> setting)
            :base(thirdPartyImportRepository, setting.Get(ServiceConfigName.PULL_DEPOSIT_INFO_SETTING))
        {
            _licenseeApi = licenseeApi;
            _logger = logger;
            _trackerService = trackerService;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await GetAdjustmentLastTrackerAsync();
            var data = await _licenseeApi.GetDepositInfo(tracker.UpdateTime, tracker.Id, MAX_ROW_COUNT);
            await ImportDataAsync(data);
        }

        private async Task ImportDataAsync(IEnumerable<DepositInfo> data)
        {
            var importData = data;
            while (importData != null && importData.Any())
            {
                await ImportDataToMultipleRepoAsync(importData);
                var trackerItem = new TrackerItem
                {
                    Id = importData.Max(_ => _.DepositId),
                    UpdateTime = importData.Max(_ => _.UpdateTime.Value),
                };
                await _trackerService.SaveTrackerAsync(TableName,
                    new TrackerInfo
                    {
                        CalledTime = DateTime.Now,
                        KeyValue = JsonConvert.SerializeObject(trackerItem)
                    });

                if (importData.Count() == MAX_ROW_COUNT)
                {
                    importData = await _licenseeApi.GetDepositInfo(trackerItem.UpdateTime, trackerItem.Id, MAX_ROW_COUNT);
                }
                else
                {
                    break;
                }
            }
        }

        private async Task<TrackerItem> GetAdjustmentLastTrackerAsync()
        {
            var tracker = await _trackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var trackerItem = JsonConvert.DeserializeObject<TrackerItem>(tracker.KeyValue);
            trackerItem.UpdateTime = trackerItem.UpdateTime.AddMinutes(Settings.AdjustmentValue);
            return trackerItem;
        }

        protected override string BuildXml(IEnumerable<DepositInfo> data)
             => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;

        private sealed class TrackerItem
        {
            public int Id { get; set; }

            public DateTime UpdateTime { get; set; }
        }
    }
}
