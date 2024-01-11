using Fanex.Affiliate.User.Core.Apis;
using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.Api;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.UserPanel.PlayerTransferBalance;
using Fanex.Affiliate.User.Core.MetalModels.Api;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class PlayerTransferBalanceServiceTest
    {
        private readonly IPlayerTransferBalanceRepository _repo;
        private readonly ILicenseeApi _api;
        private readonly IAccountApi _accountApi;
        private readonly ISessionService _sessionService;
        private readonly IAppSetting _appSetting;
        private readonly IAuthRepository _authRepository;

        public PlayerTransferBalanceServiceTest()
        {
            _repo = Substitute.For<IPlayerTransferBalanceRepository>();
            _api = Substitute.For<ILicenseeApi>();
            _accountApi = Substitute.For<IAccountApi>();
            _sessionService = NSubtituteMockingHelper.FakeSessionService();
            _appSetting = Substitute.For<IAppSetting>();
            _authRepository = Substitute.For<IAuthRepository>();
        }

        [Fact]
        public void GetPlayerTransferBalanceHistory_WhenCall_ReturnCorrectData()
        {
            _repo.GetPlayerTransferBalanceHistory(Arg.Compat.Any<PlayerTransferBalanceHistoryParam>()).Returns(new PlayerTransferBalanceHistoryResult());
            var service = CreateService();

            var result = service.GetPlayerTransferBalanceHistory(new PlayerTransferBalanceHistoryParam());

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidAff_ReturnError()
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(null as PlayerTransferBalanceInfo);
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Lang.InvalidPlayer, result.Message);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidPlayer_ReturnError(int paymentAccountId, int playerId)
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(new PlayerTransferBalanceInfo { PaymentAccountId = paymentAccountId, PlayerId = playerId });
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Lang.InvalidPlayer, result.Message);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidPlayerWalletInfo_ReturnError()
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 123,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Lang.InvalidPlayer, result.Message);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidPlayerWalletInfoStatus_ReturnError()
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 1,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 2,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Lang.PlayerNotActive, result.Message);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidAffWalletInfo_ReturnError()
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 1,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Error.PaymentAccNotExist, result.Message);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCallWithInvalidAffWalletInfoStatus_ReturnError()
        {
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 1,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                },
                new CustomerBalanceInfo
                {
                    CustId = 2,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 2,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.False(result.IsOk);
            Assert.Equal(Error.PaymentAccNotActive, result.Message);
        }

        [Fact]
        public async Task GetPlayerTransferBalanceInfoAsync_WhenCall_ReturnCorrectData()
        {
            _repo.GetPlayerTransferBalanceInfo(123, "").Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            var service = CreateService();

            var result = await service.GetPlayerTransferBalanceInfoAsync(123, "");

            Assert.True(result.IsOk);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithInvalidAmount_ReturnError()
        {
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 0 });

            Assert.False(result.IsOk);
            Assert.Equal(Lang.AmountMustPositiveNumber, result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithDisabledTransfer_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = true });
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100 });

            Assert.False(result.IsOk);
            Assert.Equal(Lang.ErrAuthorized, result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithIsInternal_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            var service = CreateService();
            _sessionService.IsInternal.Returns(true);
            _sessionService.UserSession.Returns(new UserSession { UserName = "", AffId = 123 });

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100 });

            Assert.False(result.IsOk);
            Assert.Equal(Lang.ErrAuthorized, result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithInvalidPlayerWalletInfo_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 123,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100 });

            Assert.False(result.IsOk);
            Assert.Equal(Lang.InvalidPlayer, result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithInvalidAffWalletInfo_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 1,
                    ActStatus = 1,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                }
            });
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100 });

            Assert.False(result.IsOk);
            Assert.Equal(Error.PaymentAccNotActive, result.Message);
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(1000)]
        public async Task SubmitTransferBalanceRequestAsync_WhenCallWithInvalidRemainTransferLimit_ReturnError(decimal amount)
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = amount, PlayerExRate = 0.5m });

            Assert.False(result.IsOk);
            Assert.Equal(Lang.AmountMustNotExceedLimit, result.Message);
        }

        [Theory]
        [InlineData(101, "Exchange rate has just been updated, please submit again.")]
        [InlineData(102, "You have a pending transaction. Please wait until the pending transaction completes and try again.")]
        [InlineData(123, "test")]
        [InlineData(234, null)]
        public async Task SubmitTransferBalanceRequestAsync_WhenCall_InsertFailed_ReturnError(int errCode, string message)
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, errCode, message));
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

            Assert.False(result.IsOk);
            Assert.Equal(message ?? "", result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCall_TransferMoneyFailed_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, 0, ""));
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            _api.TransferMoneyToPlayerAsync(Arg.Any<TransferMoneyParam>()).Returns(Task.FromResult(new ApiResultInfo { IsSuccess = false, ErrMsg = "Errored!" }));
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

            Assert.False(result.IsOk);
            Assert.Equal("Errored!", result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCall_TransferMoneyThrowException_ReturnError()
        {
            MockingHelper.SetUpLogEngineEnviroment();
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, 0, ""));
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            _api.TransferMoneyToPlayerAsync(Arg.Any<TransferMoneyParam>()).Returns(Task.FromResult(null as ApiResultInfo));
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

            Assert.False(result.IsOk);
            Assert.Equal(Error.ErrInternalError, result.Message);
        }

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCall_UpdateTransferFailed_ReturnError()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, 0, ""));
            _repo.UpdateTransferBalanceRequest(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns((1, "Failed!"));
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            _api.TransferMoneyToPlayerAsync(Arg.Any<TransferMoneyParam>()).Returns(Task.FromResult(new ApiResultInfo { IsSuccess = true }));
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

            Assert.False(result.IsOk);
            Assert.Equal("Failed!", result.Message);
        }

        //[Fact]
        //public async Task SubmitTransferBalanceRequestAsync_WhenCall_ActiveStatusReturnExcetion_SubmitSuccessfully()
        //{
        //    var playerInfo = playerTransferBalanceInfo;
        //    playerInfo.PlayerId = 2;
        //    _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
        //    _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerInfo);
        //    _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, 0, ""));
        //    _repo.UpdateTransferBalanceRequest(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns((0, ""));
        //    _accountApi.ActiveCustActStatus(Arg.Compat.Any<AccountParam>()).Returns(null as AccountApiResult);
        //    _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
        //    _api.TransferMoneyToPlayerAsync(Arg.Any<TransferMoneyParam>()).Returns(Task.FromResult(new ApiResultInfo { IsSuccess = true }));
        //    var service = CreateService();

        //    var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

        //    Assert.True(result.IsOk);
        //    Assert.Equal(Lang.TransferSuccessful, result.Message);
        //}

        [Fact]
        public async Task SubmitTransferBalanceRequestAsync_WhenCall_SubmitSuccessfully()
        {
            _authRepository.GetUserInfo(Arg.Compat.Any<string>(), false).Returns(new AuthorizedUserInfo { DisabledTransfer = false });
            _repo.GetPlayerTransferBalanceInfo(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns(playerTransferBalanceInfo);
            _repo.InsertTransferBalanceRequest(Arg.Compat.Any<PlayerTransferBalanceInsParam>()).Returns((12, 0, ""));
            _repo.UpdateTransferBalanceRequest(Arg.Compat.Any<int>(), Arg.Compat.Any<string>()).Returns((0, ""));
            _accountApi.ActiveCustActStatus(Arg.Compat.Any<AccountParam>()).Returns(new AccountApiResult { Data = false });
            _api.GetCustBalanceAsync(Arg.Any<int[]>()).Returns(custBalances);
            _api.TransferMoneyToPlayerAsync(Arg.Any<TransferMoneyParam>()).Returns(Task.FromResult(new ApiResultInfo { IsSuccess = true }));
            var service = CreateService();

            var result = await service.SubmitTransferBalanceRequestAsync(new TransMoneyRequestViewModel { Amount = 100, PlayerExRate = 0.5m });

            Assert.True(result.IsOk);
            Assert.Equal(Lang.TransferSuccessful, result.Message);
        }

        private PlayerTransferBalanceService CreateService()
            => new PlayerTransferBalanceService(_repo, _appSetting, _api, _accountApi, _sessionService, _authRepository);

        private PlayerTransferBalanceInfo playerTransferBalanceInfo => new PlayerTransferBalanceInfo
        {
            PlayerId = 1,
            PaymentAccountId = 2,
            TransferLimit = 2000,
            TransferedAmount = 1000
        };

        private List<CustomerBalanceInfo> custBalances => new List<CustomerBalanceInfo> {
                new CustomerBalanceInfo
                {
                    CustId = 1,
                    ActStatus = 2,
                    Balance = 1000m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Abc"
                },
                new CustomerBalanceInfo
                {
                    CustId = 2,
                    ActStatus = 1,
                    Balance = 100m,
                    CurrencyId = 123,
                    Status = 1,
                    UserName = "Test"
                }
            };
    }
}
