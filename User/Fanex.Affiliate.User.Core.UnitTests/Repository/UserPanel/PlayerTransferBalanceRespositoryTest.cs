using Fanex.Affiliate.User.Core.DTO.UserPanel.PlayerTransferBalance;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class PlayerTransferBalanceRespositoryTest : RepositoryTestBase<PlayerTransferBalanceRespository>
    {
        [Fact]
        public void GetPlayerTransferBalanceHistory_WhenCall_QueryPlayerTransferBalanceHistory()
        {
            const string uSpKey = "Aff_TransferBalanceHistory_Sel";
            var param = new PlayerTransferBalanceHistoryParam
            {
                PlayerName = string.Empty,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };
            var db = Builder.Build(uSpKey, uSpKey);
            db.Query<PlayerTransferBalanceHistoryInfo>(param).Returns(new List<PlayerTransferBalanceHistoryInfo> {new PlayerTransferBalanceHistoryInfo
            {
                TransId = 123,
                Amount = 100,
                BasedAmount = 1,
                AffPlayerAmount = 1,
                PlayerName = string.Empty,
                Currency = string.Empty,
                TransTime = new DateTime(2020, 12, 01),
                Status = "Pending"
            } });
            var repo = CreateRepo();

            var result = repo.GetPlayerTransferBalanceHistory(param);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlayerTransferBalanceInfo_WhenCall_QueryPlayerTransferBalanceInfo()
        {
            const string uSpKey = "Aff_User_TransferBalanceInfo_Sel";
            var db = Builder.Build(uSpKey, uSpKey);
            db.QueryEntity<PlayerTransferBalanceInfo>(Arg.Any<object>()).Returns(new PlayerTransferBalanceInfo
            {
                PaymentAccountId = 123,
                PlayerId = 100,
                PaymentAccountCurrencyId = 1,
                PlayerCurrencyId = 1,
                PaymentAccountExRate = 1,
                PlayerExRate = 1,
                Balance = 1,
                TransferLimit = 1,
                TransferedAmount = 1,
                PaymentAccountName = string.Empty,
                PaymentAccountCurrency = string.Empty,
                PlayerCurrency = string.Empty
            });
            var repo = CreateRepo();

            var result = repo.GetPlayerTransferBalanceInfo(123, "");

            Assert.NotNull(result);
            Assert.Equal(0, result.RemainTransferLimit);
        }

        [Fact]
        public void InsertTransferBalanceRequest_WhenCall_InsertSuccessfully()
        {
            const string uSpKey = "Aff_TransferBalanceHistory_Ins";
            var db = Builder.Build(uSpKey, uSpKey);
            db.GetOutputParams()[Arg.Any<string>()].ReturnsForAnyArgs("123");
            var repo = CreateRepo();

            var result = repo.InsertTransferBalanceRequest(new PlayerTransferBalanceInsParam
            {
                AffId = 123,
                PaymentAccountCurrencyId = 1,
                Amount = 1,
                PaymentAccountExRate = 0.5m,
                PaymentAccountId = 1,
                PlayerCurrencyId = 1,
                PlayerExRate = 0.25m,
                PlayerId = 1
            });

            Assert.Equal(123, result.TransId);
        }

        [Fact]
        public void UpdateTransferBalanceRequest_WhenCall_InsertSuccessfully()
        {
            const string uSpKey = "Aff_TransferBalanceHistory_Upd";
            var db = Builder.Build(uSpKey, uSpKey);
            db.GetOutputParams()[Arg.Any<string>()].ReturnsForAnyArgs("123");
            var repo = CreateRepo();

            var result = repo.UpdateTransferBalanceRequest(1, "pending");

            Assert.Equal(123, result.ErrCode);
        }
    }
}
