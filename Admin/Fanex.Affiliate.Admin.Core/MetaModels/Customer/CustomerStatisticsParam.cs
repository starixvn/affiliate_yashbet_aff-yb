using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Customer
{
    public class CustomerStatisticsParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int AffId { get; set; }

        [Required]
        public DateTime? MonthDate { get; set; }
    }
}
