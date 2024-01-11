using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public interface ICommissionGroupService
    {
        IEnumerable<CommissionGroupSetting> GetCommissionGroupList(GetCommissionGroupParam param);

        ResultInfo CreateCommissionGroup(CreateUpdateCommissionGroupParam param);

        ResultInfo UpdateCommissionGroup(CreateUpdateCommissionGroupParam param);

        ResultInfo DeleteCommissionGroup(DeleteCommissionGroupParam param);
    }
}