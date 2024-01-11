using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Enum;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing
{
    public class RefLinkViewModel
    {
        public AffiliateLinkType AffLinkType { get; set; }

        public string AffTypeDisplayName { get; set; }

        public IEnumerable<Domain> Domains { get; set; }

        public IEnumerable<LandingPage> SitePages { get; set; }

    }
}
