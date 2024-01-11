using Fanex.Affiliate.User.Core.Apis;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Constants.Apis;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Api;
using Fanex.Affiliate.User.Core.DTO.UserPanel.PlayerTransferBalance;
using Fanex.Affiliate.User.Core.MetalModels.Api;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class PlayerTransferBalanceService : IPlayerTransferBalanceService
    {
        private readonly IPlayerTransferBalanceRepository _repo;
        private readonly ILicenseeApi _api;
        private readonly IAccountApi _accountApi;
        private readonly ISessionService _sessionService;
        private readonly IAppSetting _appSetting;
        private readonly IAuthRepository _authRepository;

        public PlayerTransferBalanceService(IPlayerTransferBalanceRepository playerTransferBalanceRepository,
            IAppSetting appSetting,
            ILicenseeApi licenseeApi,
            IAccountApi accountApi,
            ISessionService sessionService,
            IAuthRepository authRepository)
        {
            _repo = playerTransferBalanceRepository;
            _api = licenseeApi;
            _accountApi = accountApi;
            _sessionService = sessionService;
            _appSetting = appSetting;
            _authRepository = authRepository;
        }

        public PlayerTransferBalanceHistoryResult GetPlayerTransferBalanceHistory(PlayerTransferBalanceHistoryParam param)
            => _repo.GetPlayerTransferBalanceHistory(param);

        public async Task<ResultInfo<PlayerTransferBalanceInfo>> GetPlayerTransferBalanceInfoAsync(int affId, string playerName)
        {
            var result = new ResultInfo<PlayerTransferBalanceInfo>();

            var summaryInfo = _repo.GetPlayerTransferBalanceInfo(affId, playerName);
            if (summaryInfo == null || summaryInfo.PaymentAccountId <= 0 || summaryInfo.PlayerId <= 0)
            {
                result.IsOk = false;
                result.Message = Lang.InvalidPlayer;
                return result;
            }

            var accWalletInfos = await _api.GetCustBalanceAsync(new[] { summaryInfo.PaymentAccountId, summaryInfo.PlayerId });
            var affWalletInfo = accWalletInfos.FirstOrDefault(w => w.CustId == summaryInfo.PaymentAccountId);
            var playerWalletInfo = accWalletInfos.FirstOrDefault(w => w.CustId == summaryInfo.PlayerId);
            result = CheckPlayerAcc(playerWalletInfo);
            if (!result.IsOk)
            {
                return result;
            }
            result = CheckPaymetWalletInfo(affWalletInfo);
            if (!result.IsOk)
            {
                return result;
            }

            summaryInfo.Balance = affWalletInfo.Balance;
            result.Data = summaryInfo;

            return result;
        }

        public async Task<ResultInfo> SubmitTransferBalanceRequestAsync(TransMoneyRequestViewModel param)
        {
            var validateResult = ValidateAmountAndPermission(param.Amount);
            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var affId = _sessionService.UserSession.AffId;
            var summaryInfo = _repo.GetPlayerTransferBalanceInfo(affId, param.SubmitName);

            var accWalletInfos = await _api.GetCustBalanceAsync(new[] { summaryInfo.PaymentAccountId, summaryInfo.PlayerId });
            var playerWalletInfo = accWalletInfos.FirstOrDefault(w => w.CustId == summaryInfo.PlayerId);
            var affWalletInfo = accWalletInfos.FirstOrDefault(w => w.CustId == summaryInfo.PaymentAccountId);

            if (playerWalletInfo == null)
            {
                return GetResultInfo(Lang.InvalidPlayer);
            }
            if (affWalletInfo == null)
            {
                return GetResultInfo(Error.PaymentAccNotActive);
            }

            var baseAmount = param.Amount * param.PlayerExRate;
            if (baseAmount > summaryInfo.RemainTransferLimit || baseAmount > affWalletInfo.Balance)
            {
                return GetResultInfo(Lang.AmountMustNotExceedLimit);
            }

            var (TransId, ErrCode, ErrMsg) = _repo.InsertTransferBalanceRequest(new PlayerTransferBalanceInsParam
            {
                AffId = affId,
                Amount = param.Amount,
                PaymentAccountCurrencyId = summaryInfo.PaymentAccountCurrencyId,
                PaymentAccountId = summaryInfo.PaymentAccountId,
                PlayerCurrencyId = summaryInfo.PlayerCurrencyId,
                PaymentAccountExRate = param.PaymentAccountExRate,
                PlayerExRate = param.PlayerExRate,
                PlayerId = summaryInfo.PlayerId
            });

            if (ErrCode != 0)
            {
                var errMsg = TranslateInsTransferRequestErrCodeToErrMsg(ErrCode, ErrMsg);
                return GetResultInfo(errMsg);
            }

            try
            {
                var callApiResult = await _api.TransferMoneyToPlayerAsync(new TransferMoneyParam
                {
                    FromAmount = baseAmount,
                    FromCustId = summaryInfo.PaymentAccountId,
                    MaxTransferAmount = summaryInfo.TransferLimit,
                    RefTransId = TransId.ToString(),
                    SiteId = _appSetting.MemberSiteId,
                    ToAmount = param.Amount,
                    ToCustId = summaryInfo.PlayerId
                });

                if (!callApiResult.IsSuccess)
                {
                    _repo.UpdateTransferBalanceRequest(TransId, TransferBalanceStatus.Failed);
                    return GetResultInfo(callApiResult.ErrMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"Call api to transfer money fail - {ex.Message}");
                _repo.UpdateTransferBalanceRequest(TransId, TransferBalanceStatus.Failed);
                return GetResultInfo(Error.ErrInternalError);
            }


            var updResult = _repo.UpdateTransferBalanceRequest(TransId, TransferBalanceStatus.Completed);
            if (updResult.ErrCode == 0)
            {
                if (playerWalletInfo.ActStatus != ApiConstant.DEPOSIT_ACT_STATUS)
                {
                    await ActiveDepositActStatusAsync(playerWalletInfo.CustId).ConfigureAwait(true);
                }

                return GetResultInfo(Lang.TransferSuccessful, true);
            }
            return GetResultInfo(updResult.ErrMsg);

        }

        private ResultInfo ValidateAmountAndPermission(decimal amount)
        {
            if (amount <= 0)
            {
                return GetResultInfo(Lang.AmountMustPositiveNumber);
            }

            var loginInfo = _authRepository.GetUserInfo(_sessionService.UserSession.UserName, _sessionService.IsInternal);
            if (_sessionService.IsInternal || loginInfo.DisabledTransfer)
            {
                return GetResultInfo(Lang.ErrAuthorized);
            }

            return new ResultInfo { IsOk = true };
        }

        private async Task ActiveDepositActStatusAsync(int custId)
        {
            var param = new AccountParam
            {
                CustId = custId,
                Operator = ApiConstant.OPERATOR
            };
            try
            {
                var result = await _accountApi.ActiveCustActStatus(param);

                if (!result.Data)
                {
                    await LogHelper.WriteErrorAsync($"Failed active deposit ActStatus of player {custId}\n" +
                        $"Return result: {JsonConvert.SerializeObject(result)}");
                }
            }
            catch (Exception ex)
            {
                await LogHelper.WriteErrorAsync($"Failed active deposit ActStatus of player {custId}", ex);
            }

        }

        private static string TranslateInsTransferRequestErrCodeToErrMsg(int errCode, string defaultErrMsg)
        {
            const int EXCHANGE_RATE_CHANGE = 101;
            const int HAS_PENDING_TRANS = 102;

            switch (errCode)
            {
                case EXCHANGE_RATE_CHANGE:
                    return Lang.ExchageRateChange;
                case HAS_PENDING_TRANS:
                    return Lang.HasPendingTransaction;
                default:
                    return $"{defaultErrMsg}";
            }
        }

        private static ResultInfo<PlayerTransferBalanceInfo> CheckPlayerAcc(CustomerBalanceInfo balanceInfo)
        {
            var result = new ResultInfo<PlayerTransferBalanceInfo>
            {
                IsOk = true
            };
            if (balanceInfo == null)
            {
                result.IsOk = false;
                result.Message = Lang.InvalidPlayer;
                return result;
            }

            if (balanceInfo.Status != CustomerStatus.RUNNING)
            {
                result.IsOk = false;
                result.Message = Lang.PlayerNotActive;
                return result;
            }

            return result;
        }

        private static ResultInfo<PlayerTransferBalanceInfo> CheckPaymetWalletInfo(CustomerBalanceInfo affWalletInfo)
        {
            var result = new ResultInfo<PlayerTransferBalanceInfo>
            {
                IsOk = true
            };
            if (affWalletInfo == null)
            {
                result.IsOk = false;
                result.Message = Error.PaymentAccNotExist;
                return result;
            }

            if (affWalletInfo.Status != CustomerStatus.RUNNING)
            {
                result.IsOk = false;
                result.Message = Error.PaymentAccNotActive;
                return result;
            }

            return result;
        }

        private static ResultInfo GetResultInfo(string message, bool isOk = false)
        {
            return new ResultInfo
            {
                IsOk = isOk,
                Message = message
            };
        }
    }

}
