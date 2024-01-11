using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Payment
{
    public class PaymentRequestUpdateParam : AffiliateBasicInfoParam
    {
        [Range(Validation.MinId, long.MaxValue)]
        public long TransId { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransDate { get; set; }

        public string Status { get; set; }

        public string AdminDesc { get; set; }

        public string AffDesc { get; set; }

        public bool IsApproved { get; set; }
    }
}
