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
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks
{
    public interface IFetchTpTransferInfoDataTask : IFetchDataTask { }

    public class FetchTpTransferInfoDataTask : FetchDataForMultipleRepositoryTaskBase<TpTransferInfo>, IFetchTpTransferInfoDataTask
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;
        private readonly ITrackerService _trackerService;

        private const int MAX_ROW_COUNT = 2000;

        public FetchTpTransferInfoDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService trackerService
            , IOptionsSnapshot<FetchServiceSetting> setting ) 
            : base(thirdPartyImportRepository, setting.Get(ServiceConfigName.PULL_TPTRANSFER_INFO_SETTING))
        {
            _licenseeApi = licenseeApi;
            _logger = logger;
            _trackerService = trackerService;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await _trackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var id = string.IsNullOrWhiteSpace(tracker.KeyValue) ? 0 : int.Parse(tracker.KeyValue);
            var queryId = GetAdjustmentId(id);
            var data = await _licenseeApi.GetTpTransferInfo(queryId, MAX_ROW_COUNT);

            await ImportDataAsync(data);
        }

        private async Task ImportDataAsync(IEnumerable<TpTransferInfo> data)
        {
            var importData = data;
            while (importData != null && importData.Any())
            {
                await ImportDataToMultipleRepoAsync(importData);
                var maxId = importData.Max(d => d.TransferId);
                await _trackerService.SaveTrackerAsync(TableName,
                    new TrackerInfo
                    {
                        CalledTime = DateTime.Now,
                        KeyValue = maxId.ToString()
                    });

                if(importData.Count() == MAX_ROW_COUNT)
                {
                    importData = await _licenseeApi.GetTpTransferInfo(maxId, MAX_ROW_COUNT);
                }
                else
                {
                    break;
                }
            }    
        }

        private int GetAdjustmentId(int id)
            => id - Settings.AdjustmentValue < 0 ? 0 : id - Settings.AdjustmentValue;

        protected override string BuildXml(IEnumerable<TpTransferInfo> data)
             => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}
