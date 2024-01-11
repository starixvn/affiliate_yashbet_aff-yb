using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public class CommissionGroupService : ICommissionGroupService
    {
        private readonly ICommissionGroupRepository _commGroupRepository;

        private readonly ICommGroupCommonService _commGroupCommonService;

        public CommissionGroupService(ICommissionGroupRepository commGroupRepository, ICommGroupCommonService commGroupCommonService)
        {
            _commGroupRepository = commGroupRepository;
            _commGroupCommonService = commGroupCommonService;
        }

        public IEnumerable<CommissionGroupSetting> GetCommissionGroupList(GetCommissionGroupParam param)
        {
            var rawData = _commGroupRepository.GetCommissionGroupList(param);

            var commGroupList = rawData.CommGroupList;
            var tierSettingList = rawData.TierSettingList;
            var tierDeductionSettingList = rawData.TierDeductionSettingList;
            var p2pDeductionSettingList = rawData.P2pDeductionSettingList;

            var result = from c in commGroupList
                         join t in tierSettingList on c.GroupId equals t.GroupId into groupTierSetting
                         join d in tierDeductionSettingList on c.GroupId equals d.GroupId into groupTierDeductionSetting
                         join p in p2pDeductionSettingList on c.GroupId equals p.GroupId into groupP2pDeductionSetting
                         select new CommissionGroupSetting
                         {
                             RowIdx = c.RowIdx,
                             GroupId = c.GroupId,
                             GroupName = c.GroupName,
                             SubAffCommRate = c.SubAffCommRate,
                             CreatedDate = c.CreatedDate,
                             Creator = c.Creator,
                             Description = c.Description,
                             NoAffs = c.NoAffs,
                             IsActivePlayerSum = c.IsActivePlayerSum,
                             TierSettingList = groupTierSetting,
                             P2pDeductionList = groupP2pDeductionSetting,
                             PlatformFeeList = groupTierDeductionSetting.Where(p => p.DeductionId == Constants.Deduction.PlatformFeeId),
                             DepositFee = groupTierDeductionSetting.FirstOrDefault(p => p.DeductionId == Constants.Deduction.DepositFeeId),
                             WithdrawalFee = groupTierDeductionSetting.FirstOrDefault(p => p.DeductionId == Constants.Deduction.WithdrawalFeeId)
                         };

            return result;
        }

        public ResultInfo CreateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            return _commGroupCommonService.CreateCommissionGroup(param);
        }

        public ResultInfo UpdateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            return _commGroupCommonService.UpdateCommissionGroup(param);
        }

        public ResultInfo DeleteCommissionGroup(DeleteCommissionGroupParam param)
        {
            return _commGroupRepository.DeleteCommissionGroup(param);
        }
    }
}