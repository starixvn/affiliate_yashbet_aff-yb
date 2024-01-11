using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.Login;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public interface IAuthRepository
    {
        IList<UserPermission> GetPermissionList(int userId);
        bool ReCheckin(int onUserId);

        ResultInfo Login(LoginParam param);

        int LoginOnUser(LoginParam param, string sessionId, string serverId);

        void Logout(int onUserId);

        ResultInfo CheckPermission(CheckPermissionParam param);
    }
}
