using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Affiliate
{
    public class AffiliateInfoParam
    {
        public int UserId { get; set; }

        public string AffCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string GroupIds { get; set; }

        public string Language { get; set; }

        public string Status { get; set; }

        public string ParentAffName { get; set; }

        public string UserName { get; set; }

        public string AffPlayer { get; set; }

        public string Email { get; set; }

        public byte SortBy { get; set; }

        public bool SortDirectionType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsExportExcel { get; set; }
    }
}
