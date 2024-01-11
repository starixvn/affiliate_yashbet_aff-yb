using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public interface ICommGroupAssignmentService
    {
        IEnumerable<SelectItem> GetNormalCommGroupList(GetGroupListParam param);

        Task<ResultInfo> AssignGroup(AssignGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam);

        Task<ResultInfo> AssignPrivateGroup(CreateUpdateCommissionGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam);
    }
}