using Fanex.Affiliate.Admin.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class LandingPageStatisticsParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int AffId { get; set; }

        public string SelectedPage { get; set; }

        public string LanguageCode { get; set; }

        [Required]
        public DateTime? FromDate { get; set; }

        [Required]
        public DateTime? ToDate { get; set; }

        public byte DeviceType { get; set; }
    }
}
