using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.ChangeAdminPassword;

namespace Fanex.Affiliate.Admin.Core.Services.ChangeAdminPassword
{
    public interface IChangeAdminPasswordService
    {
        ResultInfo ChangePassword(ChangeAdminPasswordParam param);
    }
}
