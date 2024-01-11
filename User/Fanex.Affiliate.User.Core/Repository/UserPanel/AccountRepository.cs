using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly IObjectDBBuilder _builder;

        public AccountRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public AccountInformation GetAccountInfo(int affId)
        {
            const string uSpKey = "Aff_User_UserInfoSel";

            using (var objectDb = _builder.Build(uSpKey))
            {
                var parameterSet = new { affId };

                return objectDb.QueryEntity<AccountInformation>(parameterSet);
            }
        }

        public ResultInfo UpdateAccountInformation(UpdateAccountParam accountUpdateParam)
        {
            const string uSpKey = "Aff_User_UserInfoUpd";

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.ExecuteNonQuery(accountUpdateParam);
            }

            return new ResultInfo
            {
                IsOk = accountUpdateParam.ErrCode == 0,
                Message = accountUpdateParam.ErrMsg
            };
        }

        public ResultInfo UpdatePassword(UpdatePasswordParam updatePasswordParam)
        {
            const string uSpKey = "Aff_AffiliateUpdPwd";
            var parameterSet = new
            {
                updatePasswordParam.AffId,
                updatePasswordParam.UserId,
                updatePasswordParam.Ip,
                updatePasswordParam.Language,
                updatePasswordParam.ErrCode,
                updatePasswordParam.ErrMsg,
                OldPwd = updatePasswordParam.OldPassword,
                AffPwd = updatePasswordParam.NewPassword
            };

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.ExecuteNonQuery(parameterSet);
            }

            return new ResultInfo
            {
                IsOk = parameterSet.ErrCode == 0,
                Message = parameterSet.ErrMsg
            };
        }

        public UserInfo GetUserInfoByEmail(UserInfoByEmailParam userInfoParam)
        {
            const string uSpKey = "Aff_User_ForgotPassword";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.QueryEntity<UserInfo>(userInfoParam);
            }
        }

        public UserInfo RecoverPassword(PasswordRecoveryParam passwordRecoveryParam)
        {
            const string uSpKey = "Aff_User_RecoverPassWord";

            using (var objectDb = _builder.Build(uSpKey))
            {
                return objectDb.QueryEntity<UserInfo>(passwordRecoveryParam);
            }
        }

        public UserInfo GetUserInfoById(int affId, bool isInternal)
        {
            const string uSpKey = "Aff_User_GeneralInfo_Sel";
            var mode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, mode))
            {
                return objectDb.QueryEntity<UserInfo>(new { affId });
            }
        }

        public ResultInfo CreateAffiliate(UserSignUpParam signUpParam)
        {
            const string uSpKey = "Aff_User_Affiliate_Ins";

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.ExecuteNonQuery(signUpParam);
            }

            return new ResultInfo
            {
                IsOk = signUpParam.ErrCode == 0,
                Message = signUpParam.ErrMsg
            };
        }

        public IEnumerable<CommissionSetting> GetCommissionSettingList(int affId, bool isInternal)
        {
            const string uSpKey = "Aff_User_CommSetting_Sel";
            var mode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, mode))
            {
                return objectDb.Query<CommissionSetting>(new { affId });
            }
        }

        public int CreateSecretKey(int affId, string secretKey)
        {
            const string uSpKey = "Aff_User_SecretKeyIns";

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.SetParameterValue("AffId", affId);
                objectDb.SetParameterValue("SecretKey", secretKey);
                AddOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                var code = objectDb.GetParameterValue("ErrCode");

                return code == DBNull.Value ? 0 : Convert.ToInt32(code);
            }
        }

        public GeneralAffInfo GetGeneralAffInfo(int? affId = null, string affCode = null)
        {
            const string uSpKey = "Aff_Get_GeneralAffInfo";

            using (var objectDb = _builder.Build(uSpKey))
            {
                var parameterSet = new { affId, affCode };

                return objectDb.QueryEntity<GeneralAffInfo>(parameterSet);
            }
        }
    }
}