using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Report
{
    public class TransferReportParam
    {
        [MaxLength(Validation.MaxLengthInputId, ErrorMessage = "Trans. Id is invalid!")]
        [RegularExpression("^\\d*$", ErrorMessage = "Trans. Id is invalid!")]
        public string TransId { get; set; }

        [MaxLength(Validation.MaxLengthInputString, ErrorMessage = "Affiliate must shorter than 30 characters!")]
        public string AffName { get; set; }

        [MaxLength(Validation.MaxLengthInputString, ErrorMessage = "Player must shorter than 30 characters!")]
        public string PlayerName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int BaseCurrencyId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsExportExcel { get; set; }
    }
}
