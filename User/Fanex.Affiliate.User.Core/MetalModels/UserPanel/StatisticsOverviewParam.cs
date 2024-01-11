using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class StatisticsOverviewParam : BaseParam
    {
        public int AffId { get; set; }

        public DateTime MonthDate { get; set; }
    }
}
