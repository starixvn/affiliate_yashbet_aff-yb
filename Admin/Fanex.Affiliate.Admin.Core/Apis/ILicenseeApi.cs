using Fanex.Affiliate.Admin.Core.DTO.Api;
using Refit;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Apis
{
    public interface ILicenseeApi
    {
        [Get("/api/customer/GetLoginCustomerInfo")]
        Task<MemberSiteCustomerInfo> GetCustomerInfoAsync(string userName, int siteId);

        [Post("/api/customer/UpdateCustomerWallet")]
        Task<decimal> UpdatePaymentRequestAsync(PaymentRequestInfo paymentInfo);

        [Post("/api/customer/UnlockUser")]
        Task<ApiResultInfo> UnlockUser(int[] userIds);

        [Post("/api/customer/InsertAffiliateInfo")]
        Task<ApiResultInfo> InsertAffiliateInfo(ApiAffiliateInfo affiliateInfo);
    }
}
