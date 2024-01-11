using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory
{
    public class EmailHistoryParam
    {
        [Range(Validation.Zero, Validation.MaxSearchBy)]
        public int SearchById { get; set; }

        public bool IsGetAll { get; set; }

        public string SearchText { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Range(Validation.MinPageIndex, Validation.MaxPageIndex)]
        public int PageIndex { get; set; }

        [Range(Validation.MinPageSize, Validation.MaxPageSize)]
        public int PageSize { get; set; }
    }
}
