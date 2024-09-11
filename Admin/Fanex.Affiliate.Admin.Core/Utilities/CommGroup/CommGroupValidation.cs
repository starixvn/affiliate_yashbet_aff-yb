using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Utilities.CommGroup
{
    public class CommGroupValidation : ICommGroupValidation
    {
        private const string brElementHtml = "<br />";

        private readonly IAuthRepository _authRepository;
        private readonly IAffiliateService _affiliateService;
        private readonly IProductRepository _productRepository;

        public CommGroupValidation(
            IAuthRepository authRepository,
            IAffiliateService affiliateService,
            IProductRepository productRepository)
        {
            _authRepository = authRepository;
            _affiliateService = affiliateService;
            _productRepository = productRepository;
        }

        public bool HasPermissionAssignOnAff(int userId, int affId)
        {
            var affInfo = _affiliateService.GetAffiliateBasicInfo(affId);

            if (string.IsNullOrEmpty(affInfo.AffCode))
            {
                var permissionList = _authRepository.GetPermissionList(userId);

                return permissionList.Any(i => i.FunctionName.MatchWith(Function.EditAffStatus) && i.IsHost);
            }

            return true;
        }

        public ResultInfo ValidateCommGroup(CommGroupSettingParam commGroup)
        {
            var result = ValidateAllFees(commGroup);

            var validateTierSettingList = ValidateTierSettingList(commGroup.TierSettingList);

            if (!validateTierSettingList.IsSuccessful)
            {
                result.Code = ErrCode.GeneralErrorCode;
                result.Message = string.Join(result.Message, validateTierSettingList.Message);
            }

            return result;
        }

        private ResultInfo ValidateAllFees(CommGroupSettingParam commGroup)
        {
            var result = new ResultInfo();
            var productIdOfDB = _productRepository.GetProductList().Select(p => p.ProductId.ToString());

            var validateP2pDeductionList = ValidateDeductionList(commGroup.P2pDeductionList, productIdOfDB, CommGroupErrorMessage.InvalidP2pDeduction);
            if (!validateP2pDeductionList.IsSuccessful)
            {
                UpdateResult(result, validateP2pDeductionList.Message);
            }

            var validatePlatformFeeList = ValidateDeductionList(commGroup.PlatformFeeList, productIdOfDB, CommGroupErrorMessage.InvalidPlatformFee);
            if (!validatePlatformFeeList.IsSuccessful)
            {
                UpdateResult(result, validatePlatformFeeList.Message);
            }

            var validateDepositFee = ValidateFixedAmountDeductedFee(commGroup.DepositFee, CommGroupErrorMessage.InvalidDepositFee);
            if (!validateDepositFee.IsSuccessful)
            {
                UpdateResult(result, validateDepositFee.Message);
            }

            var validateWithdrawalFee = ValidateFixedAmountDeductedFee(commGroup.WithdrawalFee, CommGroupErrorMessage.InvalidWithdrawalFee);
            if (!validateWithdrawalFee.IsSuccessful)
            {
                UpdateResult(result, validateWithdrawalFee.Message);
            }

            return result;
        }

        private static ResultInfo ValidateTierSettingList(IEnumerable<TierSettingParam> tierSettingList)
        {
            var result = new ResultInfo();

            if (tierSettingList == null || !tierSettingList.Any())
            {
                UpdateResult(result, "Tier Setting must have at least 1 tier!");
                return result;
            }

            if (tierSettingList.Any(item => item.MinAmount > Validation.InfinityAmount))
            {
                UpdateResult(result, "Min Amount must be lesser than or equal to 100,000,000,000!");
                return result;
            }

            if (tierSettingList.Any(item => item.MinAp > Validation.InfinityQualifiedMember))
            {
                UpdateResult(result, "Min AP must be lesser than or equal to 1,000,000,000!");
            }

            return result;
        }
        private ResultInfo ValidateDeductionList(IEnumerable<DeductionSettingParam> deductionList, IEnumerable<string> productIdOfDB, string message)
        {
            var result = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = message
            };

            if (deductionList == null || !deductionList.Any())
            {
                return result;
            }

            if (deductionList.Any(item => string.IsNullOrEmpty(item.ProductIds)))
            {
                return deductionList.Count() == Validation.OnlyOneRow ? new ResultInfo() : result;
            }

            var productIdOfDeduction = string.Join(",", deductionList.Select(p => p.ProductIds.Replace(" ", "")))
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var distinctProductIdOfPlatform = productIdOfDeduction.Distinct();

            if (distinctProductIdOfPlatform.Any(item => !productIdOfDB.Contains(item)))
            {
                return result;
            }

            return new ResultInfo();
        }

        private static ResultInfo ValidateFixedAmountDeductedFee(TierDeductionSettingParam deductedFee, string message)
        {
            var result = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = message
            };

            if (!deductedFee.IsDeductedPercent)
            {
                try
                {
                    var deductedAmount = Convert.ToDecimal(deductedFee.DeductedAmount);

                    if (deductedAmount < 0 || deductedAmount > Money.AllowedMaxValue)
                    {
                        return result;
                    }
                }
                catch
                {
                    return result;
                }
            }

            return new ResultInfo();
        }

        private static void UpdateResult(ResultInfo initialResult, string message)
        {
            initialResult.Code = ErrCode.GeneralErrorCode;
            initialResult.Message = string.Join(initialResult.Message + brElementHtml, message);
        }
    }
}