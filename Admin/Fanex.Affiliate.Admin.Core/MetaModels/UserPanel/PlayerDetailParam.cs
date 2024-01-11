using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class PlayerDetailParam : AffiliatePagingParam
    {
        private const int Turnover = 5;

        public byte ProductId { get; set; }

        public int BaseCurrencyId { get; set; }

        [Required]
        public DateTime MonthDate { get; set; }

        public bool IsExportExcel { get; set; }

        public bool IsSortAsc { get; set; }

        public int SortById { get; set; } = Turnover;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrencyIds { get; set; }
    }
}
