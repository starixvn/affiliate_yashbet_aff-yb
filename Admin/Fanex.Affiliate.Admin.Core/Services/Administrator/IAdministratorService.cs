using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Administrator;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Administrator
{
    public interface IAdministratorService
    {
        IEnumerable<PolicyInfo> GetAccountPolicyList(int groupId);

        IEnumerable<PolicyInfo> GetPolicyListByGroupIds(string groupIds);

        IEnumerable<AdministratorInfo> GetUserList(int groupId, string userName, string firstLetter);

        AdministratorInfo GetUserDetail(int userId);

        ResultInfo InsertUser(InsertUserParam param);

        ResultInfo InsertExistedUser(InsertExistedUserParam param);

        ResultInfo UpdateUser(UpdateUserParam param);

        ResultInfo ChangeStatus(ChangeStatusParam param);

        ResultInfo UpdateAccountPolicy(UpdatePolicyParam param);

        ResultInfo ChangePassword(UpdatePasswordParam param);
    }
}
