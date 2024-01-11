using Affiliate.YB.DTOs;
using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Constants;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Affiliate.YB.Utitlities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Bonus
{
    public interface IFetchWeeklyBonusInfoDataTask : IFetchDataTask { }
    public class FetchWeeklyBonusInfoDataTask : FetchDataForMultipleRepositoryTaskBase<BonusDTO>, IFetchWeeklyBonusInfoDataTask
    {
        private const int SUBSTRACT_DATE = -39;

        private readonly ILicenseeApi _licenseeApi;
        private readonly ITrackerService _trackerService;

        public FetchWeeklyBonusInfoDataTask(ILicenseeApi licenseeApi,
            IThirdPartyImportRepository _thirdPartyRepo,
            ITrackerService timeTrackerService,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(_thirdPartyRepo, configuration.Get(ServiceConfigName.PULL_WEEKLY_BONUS_INFO_SETTING))
        {
            _licenseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
            _trackerService = timeTrackerService;
        }

        public override async Task FetchDataAsync()
        {
            var date = GetLastMonthDate();
            var now = DateTime.UtcNow;
            while (date <= now)
            {
                await PullAndImportData(date);
                date = date.AddDays(1);
            }
        }

        private async Task PullAndImportData(DateTime date)
        {
            var isRunFirstTime = true;
            var listData = Enumerable.Empty<BonusDTO>();
            var pageIndex = 0;
            while(isRunFirstTime || listData.Count() == 2000)
            {
                isRunFirstTime= false;
                listData = await _licenseeApi.GetBonusInfoByDate(date, pageIndex, SettingConstant.MAX_ROW_COUNT);
                await ImportDataToMultipleRepoAsync(listData);
                var trackerItem = new { Date = date, PageIndex = pageIndex };
                await _trackerService.SaveTrackerAsync(TableName,
                    new TrackerInfo
                    {
                        CalledTime = DateTime.Now,
                        KeyValue = JsonConvert.SerializeObject(trackerItem)
                    });
                pageIndex++;
            }
        }

        private DateTime GetLastMonthDate()
            => DateTime.Now.AddDays(SUBSTRACT_DATE).Date;
        

        protected override string BuildXml(IEnumerable<BonusDTO> data)
             => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}
