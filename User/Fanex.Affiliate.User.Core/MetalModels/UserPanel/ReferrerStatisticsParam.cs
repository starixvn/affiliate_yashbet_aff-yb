namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class ReferrerStatisticsParam : StatisticsBySiteBaseParam
    {
        public int TotalClicks { get; set; }

        public int UniqueClicks { get; set; }

        public int TotalSignUps { get; set; }

        public int TotalFdms { get; set; }
    }
}
