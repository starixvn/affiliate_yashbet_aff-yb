using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.ChangeAdminPassword;
using Fanex.Affiliate.Admin.Core.Repository;

namespace Fanex.Affiliate.Admin.Core.Services.ChangeAdminPassword
{
    public class ChangeAdminPasswordService : BaseRepository, IChangeAdminPasswordService
    {
        public ResultInfo ChangePassword(ChangeAdminPasswordParam param)
        {
            const string SpKey = "Aff_Admin_ChangePassword";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("oldpwd", param.OldPassword);
                objectDb.SetParameterValue("NewPwd", param.NewPassword);
                objectDb.SetParameterValue("CustId", param.UserId);
                objectDb.SetParameterValue("UserID", param.UserId);
                objectDb.SetParameterValue("IP", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }
    }
}
