using Fanex.Affiliate.User.Core.DTO.Api;
using Fanex.Affiliate.User.Core.MetalModels.Api;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.Apis
{
    public interface ILicenseeApi
    {
        [Get("/api/customer/GetLoginCustomerInfo")]
        Task<MemberSiteCustomerInfo> GetCustomerInfoAsync(string userName, int siteId);

        [Post("/api/customer/UnlockUser")]
        Task<ApiResultInfo> UnlockUser(int[] userIds);

        [Get("/api/customer/GetCustBalance")]
        Task<IEnumerable<CustomerBalanceInfo>> GetCustBalanceAsync([Query(CollectionFormat.Multi)] int[] ids);

        [Post("/api/Customer/TransferMoneyToPlayer")]
        Task<ApiResultInfo> TransferMoneyToPlayerAsync(TransferMoneyParam param);
    }
}
