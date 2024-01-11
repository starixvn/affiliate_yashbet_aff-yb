
using Fanex.Affiliate.User.Core.DTO;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing
{
    public class QrCodeLinkViewModel
    {
        public string Link { get; set; }

        public string QrCode { get; set; }

        public Language Language { get; set; }

        public string DisplayName { get; set; }

        public bool IsShow { get; set; }
    }
}
