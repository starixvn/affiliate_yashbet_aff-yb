using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Utilities;
using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class PlayerDetailParam
    {
        private const int Turnover = 5;

        public int AffId { get; set; }

        public int PageIndex { get; set; } = Page.PageIndex;

        public int PageSize { get; set; } = Page.PageSize;

        public DateTime MonthDate { get; set; } = DateTimeHelper.NowDateTime;

        public int SortById { get; set; } = Turnover;

        public bool IsSortAsc { get; set; }

        public int NRows { get; set; }
    }
}
