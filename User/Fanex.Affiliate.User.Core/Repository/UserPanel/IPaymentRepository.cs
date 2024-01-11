using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> GetPaymentList(PaymentParam paymentParam);

        ResultInfo CancelPaymentRequest(PaymentCancelParam cancelParam);

        ResultInfo CreatePaymentRequest(PaymentRequestInfo paymentCreationParam);

        (IEnumerable<PaymentMethodInfo> PaymentMethods, decimal MinAmount) GetPaymentMethods();
    }
}
