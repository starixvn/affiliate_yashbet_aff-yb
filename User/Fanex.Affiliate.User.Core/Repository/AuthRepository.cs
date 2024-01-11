using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IObjectDBBuilder _builder;

        public AuthRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }

        public int GetOnUserId(OnUserIdParam onUserIdParam)
        {
            const string spKey = "Aff_CheckIn";
            var dbMode = onUserIdParam.IsInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(spKey, dbMode))
            {
                objectDb.ExecuteNonQuery(onUserIdParam);
            }

            return onUserIdParam.On_UserId;
        }

        public AuthorizedUserInfo GetUserInfo(string userName, bool isInternal)
        {
            const string spKey = "Aff_User_Login_UserInfoSel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(spKey, dbMode))
            {
                objectDb.SetParameterValue("UserName", userName);

                using (var reader = objectDb.ExecuteReader())
                {
                    return reader.Read() ? new AuthorizedUserInfo
                    {
                        AffId = reader.ToInt("AffId"),
                        AffCode = reader.ToInt("AffCode"),
                        UserName = reader.ToString("UserName"),
                        Password = reader.ToString("Password"),
                        GroupName = reader.ToString("GroupName"),
                        AffType = reader.ToString("AffType"),
                        Status = reader.ToString("Status"),
                        PreferredLanguage = reader.ToString("PreferredLanguage"),
                        IsMaxSubLevel = reader.ToBool("IsMaxSubLevel"),
                        IsDepositFeePercent = reader.ToBool("Dep_IsDeductedPercent"),
                        IsWithdrawalFeePercent = reader.ToBool("Wit_IsDeductedPercent"),
                        DisabledTransfer = reader.ToBool("DisabledTransfer")
                    } : null;
                }
            }
        }

        public LoginResultInfo UpdateLoginResult(LoginResultParam loginResultParam)
        {
            const string spKey = "Aff_User_Login_ResultUpd";

            using (var objectDb = _builder.Build(spKey))
            {
                AssignUpdateLoginResultParam(objectDb, loginResultParam);
                objectDb.ExecuteNonQuery();

                var code = objectDb.GetParameterValue("ErrCode");

                return new LoginResultInfo
                {
                    IsOk = code == null || Convert.ToInt32(code) == 0,
                    Message = Convert.ToString(objectDb.GetParameterValue("ErrMsg"))
                };
            }
        }

        public string GetSessionId(int onUserId)
        {
            const string spKey = "Aff_SessionIdSel";

            using (var objectDb = _builder.Build(spKey))
            {
                objectDb.SetParameterValue("OnUserId", onUserId);

                return objectDb.ExecuteScalar() as string;
            }
        }

        public bool Recheckin(int onUserId, bool isInternal)
        {
            const string spKey = "Aff_Recheckin";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var parameterSet = new
            {
                On_UserId = onUserId,
                IsKickedOut = true
            };

            using (var objectDb = _builder.Build(spKey, dbMode))
            {
                objectDb.ExecuteNonQuery(parameterSet);
            }

            return parameterSet.IsKickedOut;
        }

        private static void AssignUpdateLoginResultParam(IObjectDb objectDb, LoginResultParam param)
        {
            objectDb.SetParameterValue("AffId", param.AffId);
            objectDb.SetParameterValue("IpAddress", param.IpAddress);
            objectDb.SetParameterValue("DeviceInfo", param.DeviceInfo);
            objectDb.SetParameterValue("Language", param.Language);
            objectDb.SetParameterValue("IsSuccessful", param.IsSuccessful);

            objectDb.SetParameterValue("ErrCode", DefaultValue.Int);
            objectDb.SetParameterValue("ErrMsg", DefaultValue.String);
        }
    }
}