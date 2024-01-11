using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Payment
{
    public class PaymentRequestParamFromHome : ParamFromHome
    {
        public int SearchBy { get; set; }

        public string FromDate { get; set; }

        public IEnumerable<SubMenu> SubMenuList { get; set; }
    }
}
