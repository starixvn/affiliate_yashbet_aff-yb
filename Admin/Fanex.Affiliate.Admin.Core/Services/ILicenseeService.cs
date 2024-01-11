using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public interface ILicenseeService
    {
        Task<MemberSiteCustomerInfo> GetMemberSiteCustomerInfo(string userName, int siteId);

        Task<ResultInfo> CheckCustomerIsValid(string userName, int siteId);

        ResultInfo CheckCustomerIsValid(MemberSiteCustomerInfo cust);

        Task<ResultInfo> UpdatePaymentRequest(PaymentRequestInfo paymentRequest);

        Task<ResultInfo> UnlockUser(int userId);
    }
}
