using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class TrackingStatisticsDetailParam : TrackingStatisticsParam
    {
        [Required]
        public DateTime SelectedMonth { get; set; }
    }
}
