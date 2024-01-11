using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Affiliate.Admin.Core.Services.CommGroup
{
    public class CommGroupCommonService : ICommGroupCommonService
    {
        private readonly ICommissionGroupRepository _commGroupRepository;

        public CommGroupCommonService(
            ICommissionGroupRepository commGroupRepository)
        {
            _commGroupRepository = commGroupRepository;
        }

        public ResultInfo CreateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            BuildXml(param);

            return _commGroupRepository.CreateCommissionGroup(param);
        }

        public ResultInfo UpdateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            BuildXml(param);

            return _commGroupRepository.UpdateCommissionGroup(param);
        }

        public IEnumerable<DeductionItem> GetDeductionList()
        {
            return _commGroupRepository.GetDeductionList().Where(d => d.DeductionId != Deduction.OperationalFeeId);
        }

        private static void BuildXml(CreateUpdateCommissionGroupParam param)
        {
            param.TierSettingXml = BuildTierSettingXml(param.TierSettingList, param.PlatformFeeList);
            var tierDeductionSettingList = param.PlatformFeeList.Concat(new[] { param.DepositFee, param.WithdrawalFee });
            param.TierDeductionsXml = BuildTierDeductionSettingXml(tierDeductionSettingList);
            param.P2pDeductionsXml = BuildP2pDeductionSettingXml(param.P2pDeductionList);
        }

        private static string BuildTierSettingXml(IEnumerable<TierSettingParam> tierSettingParamList, IEnumerable<TierDeductionSettingParam> platformFeeList)
        {
            var result = new StringBuilder();
            result.Append("<root>");

            foreach (var platformFee in platformFeeList)
            {
                var tierSettings = tierSettingParamList.Where(t => t.GroupProductId == platformFee.GroupProductId);

                foreach (var item in tierSettings)
                {
                    var productIds = platformFeeList.FirstOrDefault(p => p.GroupProductId == item.GroupProductId).ProductIds;

                    var row = $"<row id=\"{item.TierId}\" minamount=\"{item.MinAmount}\"" +
                        $" minam=\"{item.MinAp}\" commission=\"{item.Commission}\" productids=\"{productIds}\" />";
                    result.Append(row);
                }
            }

            result.Append("</root>");

            return result.ToString();
        }

        private static string BuildTierDeductionSettingXml(IEnumerable<TierDeductionSettingParam> feeDeductionSettingList)
        {
            var result = new StringBuilder();
            result.Append("<root>");

            foreach (var item in feeDeductionSettingList)
            {
                var row = $"<row id=\"{item.DeductionId}\" productids=\"{item.ProductIds}\" percent=\"{item.DeductedPercentage}\"" +
                    $" amount=\"{item.DeductedAmount}\" isdeductedpercent=\"{item.IsDeductedPercent}\" />";
                result.Append(row);
            }

            result.Append("</root>");

            return result.ToString();
        }

        private static string BuildP2pDeductionSettingXml(IEnumerable<P2PDeductionSettingParam> p2pDeductionSettingList)
        {
            var result = new StringBuilder();
            result.Append("<root>");

            foreach (var item in p2pDeductionSettingList)
            {
                var row = $"<row id=\"{item.DeductionId}\" productids=\"{item.ProductIds}\" percent=\"{item.DeductedPercentage}\"" +
                    $" commission=\"{item.Commission}\" />";
                result.Append(row);
            }

            result.Append("</root>");

            return result.ToString();
        }
    }
}