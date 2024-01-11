using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    [Serializable]

    public class StatisticsBySiteBaseParam : BaseParam
    {
        public int AffId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte DeviceType { get; set; }
    }
}
