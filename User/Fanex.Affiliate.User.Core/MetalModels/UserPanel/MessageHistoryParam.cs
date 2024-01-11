using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class MessageHistoryParam : BaseParam
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 1;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? Important { get; set; }

        public string Language { get; set; }

        public int PageSize { get; set; } = DefaultPageSize;

        public int PageIndex { get; set; } = DefaultPageIndex;

        public int TotalPages { get; set; }

        public int Nrows { get; set; }
    }
}
