using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class TrackingStatisticsDetailParam : BaseParam
    {
        public int AffId { get; set; }

        public string TrackingCode { get; set; }

        [SpParam(Ignored = true)]
        public DateTime DateMonth { get; set; }

        public int Month => DateMonth.Month;

        public int Year => DateMonth.Year;
    }
}
