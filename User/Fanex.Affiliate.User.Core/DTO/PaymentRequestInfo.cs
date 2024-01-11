using Fanex.Data;

namespace Fanex.Affiliate.User.Core.DTO
{
    public class PaymentRequestInfo
    {
        public long TransId { get; set; }

        public int AffId { get; set; }

        public decimal Amount { get; set; }

        [SpParam(SqlParamName = "IP")]
        public string IpAddress { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
