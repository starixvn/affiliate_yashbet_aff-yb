using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Constants.Apis;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.DTO.Payment;
using Fanex.Affiliate.Admin.Core.Exceptions;
using Fanex.Affiliate.Admin.Core.MetaModels.Payment;
using Fanex.Affiliate.Admin.Core.Repository.Payment;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly IAffiliateService _affiliateService;
        private readonly IEmailService _emailService;
        private readonly ILicenseeService _licenseeService;
        private readonly IAppSetting _appSetting;

        public PaymentService(
            IPaymentRequestRepository paymentRequestRepository,
            IAffiliateService affiliateService,
            ILicenseeService licenseeService,
            IEmailService emailService,
            IAppSetting appSetting)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _affiliateService = affiliateService;
            _licenseeService = licenseeService;
            _emailService = emailService;
            _appSetting = appSetting;
        }

        public EntityResult<PaymentRequest> GetPaymentRequestList(PaymentRequestParam param)
        {
            return _paymentRequestRepository.GetPaymentRequestList(param);
        }

        public PaymentRequestDetail GetPaymentRequestDetail(long transId)
        {
            return _paymentRequestRepository.GetPaymentRequestDetail(transId);
        }

        public ResultInfo UpdatePaymentRequest(PaymentRequestUpdateParam param, PaymentRequestUpdateEmail emailParam)
        {
            var affInfo = _affiliateService.GetAffiliateBasicInfo(param.AffId);
            var paymentRequestDetail = GetPaymentRequestDetail(param.TransId);
            var result = UpdatePaymentRequest(param, paymentRequestDetail);
            if (result != null && result.IsSuccessful)
            {
                SendMail(param, emailParam, affInfo, paymentRequestDetail);
            }
            return result;
        }

        public (IEnumerable<PaymentMethodInfo> paymentMethods, decimal minAmount) GetPaymentMethod()
        {
            return _paymentRequestRepository.GetPaymentMethod();
        }

        public async Task<ResultInfo> HandlePaymentRequest(PaymentRequestUpdateParam param, PaymentRequestUpdateEmail emailParam)
        {
            var siteId = _appSetting.MemberSiteId;
            var paymentRequestDetail = GetPaymentRequestDetail(param.TransId);
            var affInfo = _affiliateService.GetAffiliateBasicInfo(paymentRequestDetail.AffId);
            var cust = await _licenseeService.GetMemberSiteCustomerInfo(affInfo.PlayerUsername, siteId);
            var result = _licenseeService.CheckCustomerIsValid(cust);

            if (result.Code == CustomerStatus.LOCKED)
            {
                result = await _licenseeService.UnlockUser(cust.CustId);
            }

            if (!result.IsSuccessful)
            {
                return result;
            }

            result = UpdatePaymentRequest(param, paymentRequestDetail);

            if (result.IsSuccessful && param.IsApproved)
            {
                var paymentInfo = new PaymentRequestInfo
                {
                    Amount = paymentRequestDetail.OriginalTransAmount,
                    CustId = cust.CustId,
                    RefTransId = param.TransId.ToString(),
                    SiteId = siteId,
                    Type = (int)PaymentType.WithdrawFromAffAcc,
                    UserName = affInfo.PlayerUsername
                };

                try
                {
                    LogHelper.WriteInfo($"Call member site API to payment\n" +
                        $"Param: {JsonConvert.SerializeObject(paymentInfo)}");

                    result = await _licenseeService.UpdatePaymentRequest(paymentInfo);

                    LogHelper.WriteInfo($"Membersite return result:\n" +
                        $"Param: {JsonConvert.SerializeObject(paymentInfo)}\n" +
                        $"Result: {JsonConvert.SerializeObject(result)}");
                }
                catch (Exception ex)
                {
                    throw new LicenseetException(paymentInfo, ex);
                }
            }

            if (result.IsSuccessful)
            {
                SendMail(param, emailParam, affInfo, paymentRequestDetail);
            }
            return result;
        }

        private void SendMail(PaymentRequestUpdateParam param,
            PaymentRequestUpdateEmail emailParam,
            AffiliateBasicInfo affInfo,
            PaymentRequestDetail paymentRequestDetail)
        {

            emailParam.UserSiteUrl = SiteDomainHelper.GetUserSiteUrl(affInfo.AffType);
            emailParam.FirstName = affInfo.FirstName;
            emailParam.MailTo = affInfo.Email;
            emailParam.IsHtml = !affInfo.MailOption;
            emailParam.Language = affInfo.PreferredLanguage;

            emailParam.IsApproved = param.IsApproved;
            emailParam.TransId = param.TransId;
            emailParam.TransDate = paymentRequestDetail.TransDate.ToString("MM/dd/yyyy");
            emailParam.Amount = paymentRequestDetail.Amount;

            _emailService.SendPaymentRequestUpdateEmail(emailParam);
        }

        private ResultInfo UpdatePaymentRequest(PaymentRequestUpdateParam param, PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail != null &&
                string.Equals(AffStatus.Processing, paymentRequestDetail.Status, StringComparison.InvariantCultureIgnoreCase))
            {
                param.AffId = paymentRequestDetail.AffId;
                param.Amount = paymentRequestDetail.Amount;
                param.TransDate = paymentRequestDetail.TransDate;
                param.Status = param.IsApproved ? AffStatus.Approved : AffStatus.Rejected;

                var result = _paymentRequestRepository.UpdatePaymentRequest(param);
                return result;
            }
            return new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = Lang.PaymentRequestNotValid };
        }
    }
}
