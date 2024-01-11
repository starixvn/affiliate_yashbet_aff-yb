using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.MediaGallery
{
    public class BannerListViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public IEnumerable<Category> SubCategoryList { get; set; }

        public IEnumerable<Category> SubCategoryCasinoList { get; set; }

        public IEnumerable<Category> SubCategorySportsbookList { get; set; }

        public IEnumerable<Language> LanguageList { get; set; }

        public IEnumerable<MediaGalleryFilterItem> CurrencyList { get; set; }

        public IEnumerable<MediaGalleryFilterItem> SizeList { get; set; }

        public IEnumerable<AffTypeInfo> AffTypeList { get; set; }

        public IEnumerable<SubMenu> SubMenuList { get; set; }

        public BannerParamFromHome Param { get; set; }


        public string BannerRootUrl { get; set; }
    }
}
