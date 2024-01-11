using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing
{
    public class MediaGallerySearchFormViewModel : AffiliateLinkViewModel
    {
        public IDictionary<string, IEnumerable<SelectListItem>> CategoryList { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }

        public IEnumerable<SelectListItem> SizeList { get; set; }

        public IEnumerable<SelectListItem> DeviceList { get; set; }
    }
}
