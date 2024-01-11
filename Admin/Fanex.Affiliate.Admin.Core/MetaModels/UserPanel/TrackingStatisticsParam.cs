using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class TrackingStatisticsParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int AffId { get; set; }

        public string TrackingCode { get; set; }
    }
}
