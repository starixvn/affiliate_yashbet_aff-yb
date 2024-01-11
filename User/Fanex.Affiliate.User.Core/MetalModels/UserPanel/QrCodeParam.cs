using Fanex.Affiliate.User.Core.Enum;

namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class QrCodeParam
    {
        public string SelectedLanguage { get; set; }

        public string SelectedPage { get; set; }

        public string SelectedChannel { get; set; }

        public int? SelectedDomainId { get; set; }

        public AffiliateLinkType SelectedLinkType { get; set; }
    }
}
