using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;

namespace Fanex.Affiliate.Admin.Core.Utilities.CommGroup
{
    public interface ICommGroupValidation
    {
        ResultInfo ValidateCommGroup(CommGroupSettingParam commGroup);

        bool HasPermissionAssignOnAff(int userId, int affId);
    }
}
