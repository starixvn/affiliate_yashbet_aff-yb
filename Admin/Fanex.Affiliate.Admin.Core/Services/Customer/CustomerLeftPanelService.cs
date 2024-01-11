using DNAide.Utils;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Repository.Customer;
using Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee;
using Fanex.Affiliate.Admin.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.Customer
{
    public class CustomerLeftPanelService : ICustomerLeftPanelService
    {
        private readonly IEmailService _emailService;
        private readonly IAffiliateService _affiliateService;
        private readonly ICustomerLeftPanelRepository _customerLeftPanelRepository;
        private readonly IThirdPartyLicenseeService _thirdPartyLicenseeService;

        public CustomerLeftPanelService(
            IEmailService emailService,
            IAffiliateService affiliateService,
            ICustomerLeftPanelRepository customerLeftPanelRepository,
            IThirdPartyLicenseeService thirdPartyLicenseeService)
        {
            _emailService = emailService;
            _affiliateService = affiliateService;
            _customerLeftPanelRepository = customerLeftPanelRepository;
            _thirdPartyLicenseeService = thirdPartyLicenseeService;
        }
        public AffiliateInfoOnLeftPanel GetAffiliateInfoOnLeftPanel(string affUserName, string accId, string accOnId, string sessionId)
        {
            var affiliateInfo = _customerLeftPanelRepository.GetAffiliateInfoOnLeftPanel(affUserName);
            if (affiliateInfo != null)
            {
                affiliateInfo.AffLink = GenerateUrl(affUserName, affiliateInfo.AffType, accId, accOnId, sessionId).ToString();
            }
            return affiliateInfo;
        }

        public IEnumerable<OnlineList> GetOnlineList(string affUserName)
        {
            return _customerLeftPanelRepository.GetOnlineList(affUserName);
        }

        public TransferLimitSettingInfo GetTransferLimitSetting(int affId)
        {
            return _customerLeftPanelRepository.GetTransferLimitSetting(affId);
        }

        public ResultInfo UpdateTransferLimitSetting(TransferLimitSettingParam param)
        {
            if (!param.DisabledTransfer && param.TransferLimit == 0)
            {
                return new ResultInfo
                {
                    Code = ErrCode.GeneralErrorCode,
                    Message = "Limit must be a whole positive number."
                };
            }

            return _customerLeftPanelRepository.UpdateTransferLimitSetting(param);
        }

        public ResultInfo UpdateAffType(UpdateAffTypeParam param)
        {
            return _customerLeftPanelRepository.UpdateAffType(param);
        }

        public ResultInfo UpdateClosedStatus(UpdateClosedStatusParam param)
        {
            return _customerLeftPanelRepository.UpdateClosedStatus(param);
        }

        public ResultInfo UpdateLanguage(UpdateLanguageParam param)
        {
            return _customerLeftPanelRepository.UpdateLanguage(param);
        }

        public async Task<ResultInfo> UpdateStatus(UpdateStatusParam param, AffInformationEmail emailParam)
        {
            var result = _customerLeftPanelRepository.UpdateStatus(param);

            if (result.IsSuccessful && string.Equals(param.Status, AffStatus.Disapproved, StringComparison.InvariantCultureIgnoreCase))
            {
                var affInfo = _affiliateService.GetAffiliateBasicInfo(param.AffId);

                SendDisapprovedEmail(emailParam, affInfo);
            }

            if (result.IsSuccessful && string.Equals(param.Status, AffStatus.Approved, StringComparison.InvariantCultureIgnoreCase))
            {
                var affInfo = _affiliateService.GetAffiliateBasicInfo(param.AffId);

                await TrackAndSendAffiliateToLicensee(affInfo);
            }

            return result;
        }

        private async Task TrackAndSendAffiliateToLicensee(AffiliateBasicInfo affInfo)
        {
            try
            {
                await _thirdPartyLicenseeService.SendAffiliateInfo(affInfo);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }

        }

        private void SendDisapprovedEmail(AffInformationEmail emailParam, AffiliateBasicInfo affInfo)
        {
            emailParam.UserSiteUrl = SiteDomainHelper.GetUserSiteUrl(affInfo.AffType);
            emailParam.Language = affInfo.PreferredLanguage;
            emailParam.FirstName = affInfo.FirstName;
            emailParam.MailTo = affInfo.Email;
            emailParam.IsHtml = !affInfo.MailOption;

            _emailService.SendDisapprovedEmail(emailParam);
        }

        private static Uri GenerateUrl(string custName, int affType, string accId, string accOnId, string sessionId)
        {
            var rndStr = Encryption.RandomString(6);
            var accKey = Generator.GenerateSHA512String(sessionId + rndStr);
            var url = SiteDomainHelper.GetUserInternalSiteUrl(affType);
            url += "?chalengeStr=" + rndStr;
            url += "&accKey=" + accKey;
            url += "&custName=" + custName;
            url += "&accId=" + accId;
            url += "&accOnId=" + accOnId;
            return new Uri(url, UriKind.Absolute);
        }
    }
}
