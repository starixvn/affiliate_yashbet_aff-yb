using Affiliate.YB.Core;
using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Constants;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Params;
using Affiliate.YB.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.RetryJob
{
    public interface IReInsertAffiliateInfoDataTask : IRetryDataTask { }

    public class ReInsertAffiliateInfoDataTask : IReInsertAffiliateInfoDataTask
    {
        private readonly IDataTrackerService _trackerService;
        private readonly ILicenseeApi _liceseeApi;
        private readonly ILogger _logger;

        private static IEnumerable<int> ListPullStatusId = new int[] { DataTrackerStatus.InProcess, DataTrackerStatus.Failure };
        
        private FetchServiceSetting Setting { get; }

        public ReInsertAffiliateInfoDataTask( IDataTrackerService dataTrackerService,
            ILicenseeApi licenseeApi,
            IOptionsSnapshot<FetchServiceSetting> configuration,
            ILogger logger
            ) 
        {
            _trackerService = dataTrackerService;
            _liceseeApi = licenseeApi;
            _logger = logger;
            Setting = configuration.Get(ServiceConfigName.INSERT_AFFILIATE_SETTING);
        }

        public async Task HandleAsync()
        {
            var prefixPrviders = Setting.SupportedPrefix.Select(_ => new PrefixProvider(_));
            var dataTrackerItems = await _trackerService.FetchTrackerAsync(
                prefixPrviders, 
                new int[] { DataTrackerType.InsertAffiliate }, 
                ListPullStatusId
            );

            foreach (var item in dataTrackerItems)
            {
                var affiliateInfo = JsonConvert.DeserializeObject<AffiliateInfoParam>(item.PushDataInfo);
                var result = await _liceseeApi.InsertAffiliateInfo(affiliateInfo);
                if (result.IsSuccess)
                {
                    await _trackerService.UpdateTrackerStatusAsync(item, DataTrackerStatus.Success);
                }
                else
                {
                    _logger.Error($"Insert affiliate to YB DB fail - {result.ErrMsg}");
                }
            }
        }
    }
}
