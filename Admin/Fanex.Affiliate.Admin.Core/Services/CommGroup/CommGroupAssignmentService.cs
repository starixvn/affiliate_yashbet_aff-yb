using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee;
using Fanex.Affiliate.Admin.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public class CommGroupAssignmentService : ICommGroupAssignmentService
    {
        private readonly ICommissionGroupRepository _commGroupRepository;
        private readonly IAffiliateService _affiliateService;
        private readonly IEmailService _emailService;
        private readonly ICommGroupCommonService _commGroupCommonService;
        private readonly IThirdPartyLicenseeService _thirdPartyLicenseeService;

        public CommGroupAssignmentService(
            ICommissionGroupRepository commGroupRepository,
            IAffiliateService affiliateService,
            IEmailService emailService,
            ICommGroupCommonService commGroupCommonService,
            IThirdPartyLicenseeService thirdPartyLicenseeService)
        {
            _commGroupRepository = commGroupRepository;
            _affiliateService = affiliateService;
            _emailService = emailService;
            _commGroupCommonService = commGroupCommonService;
            _thirdPartyLicenseeService = thirdPartyLicenseeService;
        }

        public IEnumerable<SelectItem> GetNormalCommGroupList(GetGroupListParam param)
        {
            return _commGroupRepository.GetNormalCommGroupList(param);
        }

        public Task<ResultInfo> AssignGroup(AssignGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam)
        {
            return ProcessAssignGroup(param, affInfo, emailParam);
        }

        public async Task<ResultInfo> AssignPrivateGroup(CreateUpdateCommissionGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam)
        {
            param.IsPrivate = param.GroupId == 0;

            var commGroupResult = param.IsPrivate ? _commGroupCommonService.CreateCommissionGroup(param) :
                                                            _commGroupCommonService.UpdateCommissionGroup(param);

            if (commGroupResult.IsSuccessful)
            {
                var assignparam = new AssignGroupParam
                {
                    UserId = param.UserId,
                    AffId = affInfo.AffId,
                    GroupId = param.GroupId,
                    Ip = param.Ip
                };

                return await ProcessAssignGroup(assignparam, affInfo, emailParam);
            }

            return commGroupResult;
        }

        private async Task<ResultInfo> ProcessAssignGroup(AssignGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam)
        {
            var assignResult = _commGroupRepository.AssignGroup(param);
            var isApproved = affInfo.GroupId != 0;
            if (!isApproved && assignResult.IsSuccessful)
            {
                return await ProcessApproveAffiliate(param, affInfo, emailParam);
            }

            return assignResult;
        }

        private async Task<ResultInfo> ProcessApproveAffiliate(AssignGroupParam param, AffInfoParam affInfo, AffInformationEmail emailParam)
        {
            var password = Generator.GetRandomString(10);
            var (approvedResult, affCode) = ApproveAffiliate(param, password);

            if (approvedResult.IsSuccessful)
            {
                await SendAffiliateInfoToLicensee(affInfo);
                SendEmailApprove(emailParam, affInfo, password, affCode);
            }

            return approvedResult;
        }

        private (ResultInfo, string) ApproveAffiliate(AssignGroupParam param, string password)
        {
            var approveParam = new ApproveAffiliateParam
            {
                UserId = param.UserId,
                AffId = param.AffId,
                Password = CryptographyHelper.Encrypt(password),
                Ip = param.Ip,
                Status = AffStatus.Approved
            };

            return _affiliateService.ApproveAffiliate(approveParam);
        }

        private async Task SendAffiliateInfoToLicensee(AffInfoParam affParam)
        {
            try
            {
                var affInfo = _affiliateService.GetAffiliateBasicInfo(affParam.AffId);

                if (affInfo != null)
                {
                    await _thirdPartyLicenseeService.SendAffiliateInfo(affInfo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        private void SendEmailApprove(AffInformationEmail emailParam, AffInfoParam affInfo, string password, string affCode)
        {
            emailParam.UserSiteUrl = SiteDomainHelper.GetUserSiteUrl(affInfo.AffType);
            emailParam.Language = affInfo.PreferredLanguage;
            emailParam.FirstName = affInfo.FirstName;
            emailParam.UserName = affInfo.UserName;
            emailParam.MailTo = affInfo.Email;
            emailParam.IsHtml = !affInfo.MailOption;
            emailParam.Password = password;
            emailParam.AffCode = affCode;

            _emailService.SendApprovedEmail(emailParam);
        }
    }
}