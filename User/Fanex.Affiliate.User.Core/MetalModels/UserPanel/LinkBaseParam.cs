using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Data;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class LinkBaseParam : BaseParam
    {
        [SpParam(Ignored = true)]
        public int AffId { get; set; }

        [SpParam(Ignored = true)]
        public int AffCode { get; set; }

        [SpParam(Ignored = true)]
        public string TextLinkTemplate { get; set; }

        [SpParam(Ignored = true)]
        public string SelectedPage { get; set; }

        [SpParam(Ignored = true)]
        public Domain SelectedDomain { get; set; }

        [SpParam(Ignored = true)]
        public int SelectedDomainId { get; set; }

        [SpParam(Ignored = true)]
        public string SelectedChannel { get; set; }

    }
}