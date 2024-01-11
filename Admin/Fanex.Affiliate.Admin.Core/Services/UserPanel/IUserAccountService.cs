using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public interface IUserAccountService
    {
        UserAccount GetUserAccountInfo(int affId);

        Task<ResultInfo> UpdateUserAccountAsync(UserAccountParam param);
    }
}
