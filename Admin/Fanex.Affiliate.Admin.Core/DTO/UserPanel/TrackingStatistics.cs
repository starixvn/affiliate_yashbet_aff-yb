using System;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    [Serializable]
    public class TrackingStatistics
    {
        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public DateTime Date { get; set; }

        public string MonthText => Date.ToString("MMMM-yyyy");

        public string DateText => Date.ToString("MM/dd/yyyy");
    }
}
