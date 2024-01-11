using Fanex.Affiliate.Admin.Core.DTO.Common;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Affiliate.Admin.Core.Services.Common
{
    public class SessionService : ISessionService
    {
        private const string UserPermissionSessionKey = "__UserPermissionSessionKey";

        private const string UserIdKey = "__CustID";
        private const string OnUserIDKey = "__OnUserIDKey";
        private const string AffNameKey = "AffName";

        public int UserId
        {
            get
            {
                var value = HttpContext.Current.Session[UserIdKey];
                return value != null ? (int)value : 0;
            }
            set
            {
                HttpContext.Current.Session[UserIdKey] = value;
            }
        }

        public int OnUserId
        {
            get
            {
                var value = HttpContext.Current.Session[OnUserIDKey];
                return value != null ? (int)value : 0;
            }
            set
            {
                HttpContext.Current.Session[OnUserIDKey] = value;
            }
        }

        public string AffName
        {
            get
            {
                var temp = HttpContext.Current.Session[AffNameKey];

                return temp == null ? string.Empty : (string)temp;
            }
            set
            {
                HttpContext.Current.Session[AffNameKey] = value;
            }
        }

        public IList<UserPermission> UserPermissionList
        {

            get
            {
                var value = HttpContext.Current.Session[UserPermissionSessionKey];
                return value != null ? value as IList<UserPermission> : new List<UserPermission>();
            }
            set
            {
                HttpContext.Current.Session[UserPermissionSessionKey] = value;
            }
        }
    }
}
