using Fanex.Affiliate.Admin.Core.Apis;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using Fanex.Affiliate.Admin.Core.DTO.Tracker;
using Fanex.Affiliate.Admin.Core.Exceptions;
using Fanex.Affiliate.Admin.Core.Repository.Tracker;
using Fanex.Affiliate.Admin.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee
{
    public class ThirdPartyLicenseeService : IThirdPartyLicenseeService
    {
        private readonly IJobTrackerRepository _trackerRepository;
        private readonly ILicenseeApi _licenseeApi;
        private readonly IAppSetting _appSetting;

        public ThirdPartyLicenseeService(IJobTrackerRepository jobTrackerRepository, ILicenseeApi licenseeApi, IAppSetting appSetting)
        {
            _trackerRepository = jobTrackerRepository;
            _licenseeApi = licenseeApi;
            _appSetting = appSetting;
        }

        public async Task SendAffiliateInfo(AffiliateBasicInfo affInfo)
        {
            var affiliateInfo = new ApiAffiliateInfo
            {
                AffiliateCode = affInfo.AffCode,
                AffiliateName = affInfo.UserName,
                UserName = affInfo.PlayerUsername,
                FirstName = affInfo.FirstName,
                LastName = affInfo.LastName,
                ContactNumber = affInfo.ContactNumber,
                Email = affInfo.Email,
                Status = affInfo.Status,
                CustId = affInfo.PlayerCustId,
                ParentAffiliate = affInfo.ParentAffiliateName,
                SiteId = _appSetting.MemberSiteId
            };
            var (Id, Result) = _trackerRepository.InsertDataTracker(new InsertDataTrackerDTO
            {
                PushDataInfo = JsonConvert.SerializeObject(affiliateInfo),
                PushDataType = DataTrackerType.ApproveAffiliate
            });
            try
            {
                if (Result.IsSuccessful)
                {
                    await SendAffiliateInfoToLicensee(affiliateInfo);

                    _trackerRepository.UpdateDataTracker(new UpdateDataTrackerDTO
                    {
                        PushTrackerId = Id,
                        StatusId = DataTrackerStatus.Success
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                _trackerRepository.UpdateDataTracker(new UpdateDataTrackerDTO
                {
                    PushTrackerId = Id,
                    StatusId = DataTrackerStatus.Failure
                });
            }
        }

        private async Task SendAffiliateInfoToLicensee(ApiAffiliateInfo affiliateInfo)
        {
            var result = await _licenseeApi.InsertAffiliateInfo(affiliateInfo);
            if (!result.IsSuccess)
            {
                throw new LicenseetException(affiliateInfo, new ResultInfo { Code = 1, Message = result.ErrMsg });
            }
        }
    }
}
