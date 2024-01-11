using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class SubAffiliateParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int AffId { get; set; }

        [Required]
        public DateTime MonthDate { get; set; }

        public int CurrencyId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}