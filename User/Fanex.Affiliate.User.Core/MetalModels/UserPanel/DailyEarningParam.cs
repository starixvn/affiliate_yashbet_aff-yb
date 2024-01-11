using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    [Serializable]
    public class DailyEarningParam : BaseParam
    {
        public int AffId { get; set; }

        public byte? ProductId { get; set; }

        public int BaseCurrency { get; set; }

        public DateTime? MonthDate { get; set; }

        [SpParam(Ignored = true)]
        public bool IsOnlyContent { get; set; }
    }
}
