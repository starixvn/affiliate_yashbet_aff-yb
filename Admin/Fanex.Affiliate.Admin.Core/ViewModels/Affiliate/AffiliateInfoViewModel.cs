using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Affiliate
{
    public class AffiliateInfoViewModel
    {
        public IEnumerable<SelectItem> CommGroupList { get; set; }

        public IEnumerable<Language> LanguageList { get; set; }

        public int SelectedGroupId { get; set; }

        public ParamFromHome Param { get; set; }

        public IEnumerable<SubMenu> SubMenuList { get; set; }
    }
}
