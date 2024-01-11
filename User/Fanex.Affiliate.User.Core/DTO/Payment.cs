using System;

namespace Fanex.Affiliate.User.Core.DTO
{
    public class Payment
    {
        public long TransId { get; set; }

        public long RefId { get; set; }

        public DateTime TransDate { get; set; }

        public DateTime CompletedDate { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public string AffDesc { get; set; }
    }
}
