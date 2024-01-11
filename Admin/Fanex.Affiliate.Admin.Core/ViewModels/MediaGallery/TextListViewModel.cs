using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.MediaGallery
{
    public class TextListViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public IEnumerable<Language> LanguageList { get; set; }

        public IEnumerable<MediaGalleryFilterItem> CurrencyList { get; set; }

        public IEnumerable<SubMenu> SubMenuList { get; set; }
    }
}
