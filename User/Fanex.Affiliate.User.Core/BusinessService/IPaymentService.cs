using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetPaymentList(PaymentParam paymentParam);

        Task<ResultInfo> CreatePaymentRequest(PaymentCreationParam paymentCreationParam, PaymentEmailParam emailParam);

        ResultInfo CancelPaymentRequest(PaymentCancelParam cancelParam, PaymentEmailParam emailParam);

        IDictionary<string, string> GetPaymentMethods();
    }
}
