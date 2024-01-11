using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Member
{
    public class MemberListViewModel
    {
        public IEnumerable<SubMenu> SubMenuList { get; set; }

        public string AffName { get; set; }

        public bool IsFromOtherPages => !string.IsNullOrEmpty(AffName);
    }
}
