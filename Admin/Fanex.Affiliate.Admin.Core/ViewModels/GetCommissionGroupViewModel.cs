using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.ViewModels
{
    public class GetCommissionGroupViewModel
    {
        public IEnumerable<CommissionGroupSetting> CommissionSettingList { get; set; }

        public int TotalRows { get; set; }
    }
}
