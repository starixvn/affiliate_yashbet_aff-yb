using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.DTO.Payment;
using Fanex.Affiliate.Admin.Core.MetaModels.Payment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.Payment
{
    public interface IPaymentService
    {
        EntityResult<PaymentRequest> GetPaymentRequestList(PaymentRequestParam param);

        (IEnumerable<PaymentMethodInfo> paymentMethods, decimal minAmount) GetPaymentMethod();

        PaymentRequestDetail GetPaymentRequestDetail(long transId);

        ResultInfo UpdatePaymentRequest(PaymentRequestUpdateParam param, PaymentRequestUpdateEmail emailParam);

        Task<ResultInfo> HandlePaymentRequest(PaymentRequestUpdateParam param, PaymentRequestUpdateEmail emailParam);
    }
}
