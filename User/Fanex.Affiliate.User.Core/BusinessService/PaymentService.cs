using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;

        public PaymentService(
            IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IEmailService emailService,
            IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _emailService = emailService;
            _accountService = accountService;
        }

        public IEnumerable<Payment> GetPaymentList(PaymentParam paymentParam)
        {
            return _paymentRepository.GetPaymentList(paymentParam);
        }

        public ResultInfo CancelPaymentRequest(PaymentCancelParam cancelParam, PaymentEmailParam emailParam)
        {
            cancelParam.Status = "Cancelled";

            var resultInfo = _paymentRepository.CancelPaymentRequest(cancelParam);

            if (resultInfo.IsOk)
            {
                var userInfo = _accountRepository.GetUserInfoById(cancelParam.AffId, cancelParam.IsInternal);

                if (userInfo != null)
                {
                    emailParam.User = userInfo;
                    emailParam.PaymentType = PaymentRequestType.Cancel;
                    emailParam.MailTo = userInfo.Email;
                }

                SendEmail(emailParam);
            }

            return resultInfo;
        }

        public async Task<ResultInfo> CreatePaymentRequest(PaymentCreationParam paymentCreationParam, PaymentEmailParam emailParam)
        {
            var resultInfo = await ValidatePaymentRequest(paymentCreationParam);

            if (resultInfo.IsOk)
            {
                var param = new PaymentRequestInfo
                {
                    AffId = paymentCreationParam.AffId,
                    Language = paymentCreationParam.Language,
                    IpAddress = paymentCreationParam.Ip,
                    Amount = paymentCreationParam.Amount,
                    ErrCode = paymentCreationParam.ErrCode,
                    ErrMsg = paymentCreationParam.ErrMsg
                };

                resultInfo = _paymentRepository.CreatePaymentRequest(param);

                if (resultInfo.IsOk)
                {
                    paymentCreationParam.TransId = param.TransId;

                    AssignEmailParam(emailParam, paymentCreationParam, PaymentRequestType.Create);
                    SendEmail(emailParam);
                }
            }

            return resultInfo;
        }

        private async Task<ResultInfo> ValidatePaymentRequest(PaymentCreationParam paymentCreationParam)
        {
            var resultInfo = new ResultInfo { IsOk = false };
            if (!IsValidAmount(paymentCreationParam))
            {
                resultInfo.Message = Error.PaymentAmountInvalid;
                return resultInfo;
            }

            var affInfo = _accountRepository.GetAccountInfo(paymentCreationParam.AffId);
            resultInfo = await _accountService.CheckAndUnlockUserIfLocked(affInfo.PlayerUsername);

            return resultInfo;
        }

        private static bool IsValidAmount(PaymentCreationParam paymentCreationParam)
        {
            return paymentCreationParam.Amount > 0 &&
                paymentCreationParam.Amount >= paymentCreationParam.MinAmount &&
                paymentCreationParam.Amount <= paymentCreationParam.BalBroughtForward;
        }

        private void AssignEmailParam(PaymentEmailParam emailParam, PaymentCreationParam paymentCreationParam, PaymentRequestType paymentType)
        {
            var userInfo = _accountRepository.GetUserInfoById(paymentCreationParam.AffId, paymentCreationParam.IsInternal);

            if (userInfo != null)
            {
                emailParam.User = userInfo;
                emailParam.PaymentType = paymentType;
                emailParam.MailTo = userInfo.Email;
                emailParam.TransId = paymentCreationParam.TransId;
                emailParam.Amount = paymentCreationParam.Amount;
                emailParam.RequestDate = paymentCreationParam.TransDate ?? DateTimeHelper.NowDateTime;
            }
        }

        private void SendEmail(PaymentEmailParam emailParam)
        {
            if (!string.IsNullOrEmpty(emailParam.MailTo))
            {
                _emailService.SendPaymentEmail(emailParam);
            }
        }

        public IDictionary<string, string> GetPaymentMethods()
        {
            var paymentMethods = _paymentRepository.GetPaymentMethods()
                .PaymentMethods
                .ToDictionary(m => m.PaymentId, m => m.PaymentName);
            return paymentMethods;
        }
    }
}
