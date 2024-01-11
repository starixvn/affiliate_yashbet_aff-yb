using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel.Statistics
{
    public class TrackingStatisticsViewModel
    {
        public IEnumerable<string> TrackingCodeList { get; set; }

        public IEnumerable<TrackingStatistics> TrackingStatisticsList { get; set; }

        public bool IsDetail { get; set; }

        public string CurrentLanguage { get; set; }
    }
}
