using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public interface IStatisticsRepository
    {
        IEnumerable<string> GetTrackingCode(int affId, bool isInternal);

        IEnumerable<TrackingStatistics> GetTrackingStatistics(int affId, string trackingCode, bool isInternal);

        IEnumerable<TrackingStatistics> GetTrackingStatisticsDetail(TrackingStatisticsDetailParam trackingStatisticsDetailParam);

        IEnumerable<ReferrerStatistics> GetReferrerStatistics(ReferrerStatisticsParam referrerStatisticsParam);

        IEnumerable<ReferrerStatistics> GetStatisticsDetailBySite(StatisticsDetailBySiteParam statisticsDetailParam);

        IEnumerable<LandingPageStatistics> GetLandingPageStatistics(LandingPageStatisticsParam landingPageStatisticsParam);

        AccountOverviewStatistics GetStatisticsOverview(StatisticsOverviewParam overviewParam);

        IEnumerable<MarketingPageStatistics> GetMarketingPageStatistics(MarketingPageStatisticsParam param);


    }
}
