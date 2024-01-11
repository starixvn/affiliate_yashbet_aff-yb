using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public interface ICommGroupCommonService
    {
        ResultInfo CreateCommissionGroup(CreateUpdateCommissionGroupParam param);

        ResultInfo UpdateCommissionGroup(CreateUpdateCommissionGroupParam param);

        IEnumerable<DeductionItem> GetDeductionList();
    }
}