using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class AccountOverviewRepositoryTest : RepositoryTestBase<AccountOverviewRepository>
    {
        [Fact]
        public void GetLastLoginInfo_WhenCall_ReturnLastLoginInfo()
        {
            const string uSpKey = "Aff_User_LoginInfo_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.QueryEntity<LastLoginInfo>(null).ReturnsForAnyArgs(new LastLoginInfo());

            var result = repo.GetLastLoginInfo(1);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlayerDetailList_WhenCall_ReturnListPlayerDetail()
        {
            const string uSpKey = "Aff_User_Overview_PlayersDetail_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.Query<PlayerDetail>(null).ReturnsForAnyArgs(new List<PlayerDetail>
            {
                new PlayerDetail
                {
                    Bonus = 100, Deposit = 100, RegisteredDate = DateTime.Now, FirstDepositDate = DateTime.Now,
                    Turnover = 100, Username = "name", GGR = 100, Withdrawal = 100,
                    DepositFee = 0, Rebate = 0, WithdrawalFee = 0
                }
            });

            var result = repo.GetPlayerDetailList(new PlayerDetailParam());

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetSubAffDetails_WhenCall_ReturnListSubAffDetail()
        {
            const string uSpKey = "Aff_User_Overview_SubAffiliate_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.Query<SubAffDetail>(null).ReturnsForAnyArgs(new List<SubAffDetail>
            {
                new SubAffDetail
                {
                    AffId = 1,
                    UserName = "Nova",
                    FirstName = "Nova",
                    LastName = "Nova",
                    IsSuspended = true,
                    ApprovedDate = DateTime.Now,
                    SuspendedDate = DateTime.Now,
                    Turnover = 10,
                    ActivePlayer = 1,
                    Commission = 10,
                    GGR = 1,
                }
            });

            var result = repo.GetSubAffDetails(new SubAffDetailParam());

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetUserOverviewDetailList_WhenCall_QueryOverviewDetail()
        {
            const string uSpKey = "Aff_User_Overview_Detail_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.Query<OverviewDetail>(null).ReturnsForAnyArgs(new List<OverviewDetail> { new OverviewDetail { WinlostDate = new DateTime(2020, 12, 01) } });

            var result = repo.GetUserOverviewDetailList(1, DateTime.Now);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetUserOverviewSummary_WhenCall_ReturnResult()
        {
            const string uSpKey = "Aff_User_Overview_Summary_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.QueryEntity<OverviewSummary>(null).ReturnsForAnyArgs(new OverviewSummary
            {
                ActivePlayers = 2,
                BalanceBroughtForward = 100,
                Commission = 10,
                Deposit = 100,
                DepositedMembers = 2,
                QualifiedActivePlayers = 1,
                Signups = 2,
                Turnover = 100,
                NoOfSubAff = 0,
                SubAffCommission = 0
            });

            var result = repo.GetUserOverviewSummary(1, DateTime.Now);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.NotNull(result);
        }
    }
}
