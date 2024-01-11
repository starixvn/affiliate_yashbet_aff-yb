using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Report
{
    public class AffiliateReportParam
    {
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string PreferredLanguage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string GroupIds { get; set; }

        public bool IsGetAll { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ProductIds { get; set; }

        public byte SortType { get; set; }

        public byte SortDirectionType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsExportExcel { get; set; }

        public int BaseCurrencyId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrencyIds { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PaymentCurrencyIds { get; set; }
    }
}
