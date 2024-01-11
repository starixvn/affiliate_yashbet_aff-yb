using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public interface IAccountRepository
    {
        AccountInformation GetAccountInfo(int affId);

        ResultInfo UpdateAccountInformation(UpdateAccountParam accountUpdateParam);

        ResultInfo UpdatePassword(UpdatePasswordParam updatePasswordParam);

        UserInfo GetUserInfoByEmail(UserInfoByEmailParam userInfoParam);

        UserInfo RecoverPassword(PasswordRecoveryParam passwordRecoveryParam);

        UserInfo GetUserInfoById(int affId, bool isInternal);

        ResultInfo CreateAffiliate(UserSignUpParam signUpParam);

        int CreateSecretKey(int affId, string secretKey);

        IEnumerable<CommissionSetting> GetCommissionSettingList(int affId, bool isInternal);

        GeneralAffInfo GetGeneralAffInfo(int? affId = null, string affCode = null);
    }
}