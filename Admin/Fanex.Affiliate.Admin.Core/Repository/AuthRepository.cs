using DNAide.Utils;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.Login;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public IList<UserPermission> GetPermissionList(int userId)
        {
            const string SpKey = "Aff_Admin_PolicySel";
            var result = new List<UserPermission>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader(new { userId }))
                {
                    while (reader.Read())
                    {
                        result.Add(new UserPermission
                        {
                            FunctionName = reader.ToString("FuncName"),
                            IsHost = Convert.ToBoolean(reader["Host"])
                        });
                    }
                }
            }

            return result.Where(p => !p.FunctionName.Contains("TransferPlayer")).ToList(); //Hide Transfer Player
        }

        public bool ReCheckin(int onUserId)
        {
            const string SpKey = "Aff_Recheckin";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.OnUserId, onUserId);
                objectDb.SetParameterValue("iskickedout", DefaultValue.Int);
                objectDb.ExecuteNonQuery();
                var isKickedOut = objectDb.GetParameterValue("iskickedout");

                return (isKickedOut != DBNull.Value) && Convert.ToBoolean(isKickedOut);
            }
        }

        public ResultInfo Login(LoginParam param)
        {
            const string SpKey = "Aff_Admin_Login";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("UserName", param.UserName);
                objectDb.SetParameterValue("UserPwd", Encryption.CfsEnCode(param.Password));
                objectDb.SetParameterValue("IP", param.Ip);
                objectDb.SetParameterValue("HostName", param.UserHostName);

                objectDb.SetParameterValue("UserId", DefaultValue.Int);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                var userId = objectDb.GetParameterValue("UserId");

                param.UserId = userId == DBNull.Value ? 0 : Convert.ToInt32(userId);

                return GetResultInfo(objectDb);
            }
        }

        public int LoginOnUser(LoginParam param, string sessionId, string serverId)
        {
            const int AffiliateAdminSite = 6;
            const string SpKey = "Aff_CheckIn";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("custid", param.UserId);
                objectDb.SetParameterValue("netsessionid", sessionId);
                objectDb.SetParameterValue("serverid", serverId);
                objectDb.SetParameterValue("ip", param.Ip);
                objectDb.SetParameterValue("subaccid", AffiliateAdminSite);

                objectDb.SetParameterValue(Field.OnUserId, DefaultValue.Int);

                objectDb.ExecuteNonQuery();

                var userId = objectDb.GetParameterValue(Field.OnUserId);

                return userId == DBNull.Value ? 0 : Convert.ToInt32(userId);
            }
        }

        public void Logout(int onUserId)
        {
            const string SpKey = "Aff_Logout";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.OnUserId, onUserId);

                objectDb.ExecuteNonQuery();
            }
        }

        public ResultInfo CheckPermission(CheckPermissionParam param)
        {
            const string spName = "Aff_Con_CheckPermission";
            using (var objectDb = GetObjectDb(spName))
            {
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery(param);

                return GetResultInfo(objectDb);
            }
        }
    }
}