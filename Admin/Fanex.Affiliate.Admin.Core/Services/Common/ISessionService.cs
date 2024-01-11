using Fanex.Affiliate.Admin.Core.DTO.Common;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Common
{
    public interface ISessionService
    {
        int UserId { get; set; }

        int OnUserId { get; set; }

        string AffName { get; set; }

        IList<UserPermission> UserPermissionList { get; set; }
    }
}