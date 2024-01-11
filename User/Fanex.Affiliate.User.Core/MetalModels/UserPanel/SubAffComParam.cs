using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class SubAffComParam : BaseParam
    {
        public int AffId { get; set; }

        public DateTime? MonthDate { get; set; }

        public int BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string RequestPage { get; set; } = Constants.RequestPage.StatementHistory;

        [SpParam(Ignored = true)]
        public bool IsOnlyContent { get; set; }
    }
}
