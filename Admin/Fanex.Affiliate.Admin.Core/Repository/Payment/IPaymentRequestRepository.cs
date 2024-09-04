using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Payment;
using Fanex.Affiliate.Admin.Core.MetaModels.Payment;

namespace Fanex.Affiliate.Admin.Core.Repository.Payment
{
    public interface IPaymentRequestRepository
    {
        EntityResult<PaymentRequest> GetPaymentRequestList(PaymentRequestParam param);

        PaymentRequestDetail GetPaymentRequestDetail(long transId);

        ResultInfo UpdatePaymentRequest(PaymentRequestUpdateParam param);
    }
}
