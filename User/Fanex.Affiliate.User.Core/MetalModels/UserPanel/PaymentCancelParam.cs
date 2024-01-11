using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class PaymentCancelParam : BaseParam
    {
        public long TransId { get; set; }

        public string Status { get; set; }

        public int AffId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public decimal Amount { get; set; }

        [SpParam(Ignored = true)]
        public DateTime RequestDate { get; set; }
    }
}
