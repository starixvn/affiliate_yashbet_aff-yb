using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class DailyEarningParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int AffId { get; set; }

        public byte ProductId { get; set; }

        [Required]
        public DateTime? MonthDate { get; set; }

        public int BaseCurrencyId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrencyIds { get; set; }
    }
}
