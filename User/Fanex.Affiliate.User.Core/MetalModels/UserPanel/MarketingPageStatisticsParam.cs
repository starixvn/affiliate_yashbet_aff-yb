using System;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class MarketingPageStatisticsParam
    {
        public int AffId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string LanguageCodes { get; set; }

        public bool IncludedInActive { get; set; }
    }
}
