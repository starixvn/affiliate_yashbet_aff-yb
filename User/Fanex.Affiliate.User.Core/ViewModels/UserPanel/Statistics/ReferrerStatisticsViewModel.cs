using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics
{
    public class ReferrerStatisticsViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<ReferrerStatistics> DataList { get; set; }

        public bool IsDetail { get; set; }

        public int RowNumber { get; set; }
    }
}
