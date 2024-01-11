using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels.Domain
{
    public class DomainViewModel
    {
        public IEnumerable<SubMenu> SubMenuList { get; set; }

        public bool IsAffDomain { get; set; }
    }
}
