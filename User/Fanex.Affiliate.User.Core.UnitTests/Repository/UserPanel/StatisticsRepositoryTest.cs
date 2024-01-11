using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class StatisticsRepositoryTest : RepositoryTestBase<StatisticsRepository>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTrackingCode_WhenCall_QueryCode(bool isInternal)
        {
            const string uSpkey = "Aff_AffiliateSelTrackingCode";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var db = Builder.Build(uSpkey, dbMode);
            db.Query<string>().ReturnsForAnyArgs(new List<string> { "" });

            var repo = CreateRepo();
            var result = repo.GetTrackingCode(1, isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTrackingStatistics_WhenCall_ReturnTrackingList(bool isInternal)
        {
            const string uSpkey = "Aff_AffiliateSelClickStatisticsAllMonths";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var db = Builder.Build(uSpkey, dbMode);
            var multipleReader = db.QueryMultiple();
            multipleReader.IsEmptyOrClosed.Returns(false, false, true);
            multipleReader.Read<TrackingStatistics>().ReturnsForAnyArgs(
                new List<TrackingStatistics>
                {
                    new TrackingStatistics { Date = DateTime.Now, Fdm = 1, SignUps = 1, TotalClick = 1, UniqueClick = 1 },
                    new TrackingStatistics { Date = DateTime.Now, Fdm = 1, SignUps = 1, TotalClick = 1, UniqueClick = 1 }
                }
                , new List<TrackingStatistics>());
            var repo = CreateRepo();

            var result = repo.GetTrackingStatistics(1, "", isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTrackingStatisticsDetail_WhenCall_QueryTrackingStatisticsDetail(bool isInternal)
        {
            const string uSpkey = "Aff_AffiliateSelClickStatisticsMonthDetail";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var param = new TrackingStatisticsDetailParam { IsInternal = isInternal };
            var db = Builder.Build(uSpkey, dbMode);
            db.Query<TrackingStatistics>(param).Returns(new List<TrackingStatistics> { new TrackingStatistics { } });
            var repo = CreateRepo();

            var result = repo.GetTrackingStatisticsDetail(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetReferrerStatistics_WhenCall_QueryReferrerStatistics(bool isInternal)
        {
            const string uSpkey = "Aff_ClickStatisticsSelBySite";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var param = new ReferrerStatisticsParam
            {
                AffId = 1,
                DeviceType = 1,
                FromDate = DateTime.Now,
                IsInternal = isInternal,
                ToDate = DateTime.Now,
                TotalClicks = 10,
                TotalFdms = 2,
                TotalSignUps = 4,
                UniqueClicks = 3
            };
            var db = Builder.Build(uSpkey, dbMode);
            db.Query<ReferrerStatistics>(param).Returns(new List<ReferrerStatistics>
            {
                new ReferrerStatistics { DomainReferrer = "", Fdm = 1, SignUps = 1, TotalClick = 1, UniqueClick = 1}
            });
            var repo = CreateRepo();

            var result = repo.GetReferrerStatistics(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStatisticDetailBySite_WhenCall_ReturnListReferreStatitstics(bool isInternal)
        {
            const string uSpkey = "Aff_ClickStatisticsDetailBySite";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var param = new StatisticsDetailBySiteParam
            {
                AffId = 1,
                DeviceType = 1,
                Domain = "",
                FromDate = DateTime.Now,
                IsInternal = isInternal,
                LanguageCode = "",
                NRows = 1,
                PageIndex = 1,
                PageSize = 1,
                SportCode = "",
                ToDate = DateTime.Now,
                TotalPages = 1,
            };
            var rowValue = DBNull.Value;
            var db = Builder.Build(uSpkey, dbMode);
            var reader = db.ExecuteReader(param);
            reader.Read().Returns(true, false);
            reader[Arg.Any<string>()].Returns(rowValue);
            var repo = CreateRepo();

            var result = repo.GetStatisticsDetailBySite(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLandingPageStatistics_WhenCall_QueryLandingPage(bool isInternal)
        {
            const string uSpkey = "Aff_ClickStatisticsSelByPage";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var param = new LandingPageStatisticsParam
            {
                SportCode = "",
                LanguageCode = "",
                TotalFdms = 1,
                TotalSignUps = 1,
                IsInternal = isInternal
            };
            var db = Builder.Build(uSpkey, dbMode);
            db.Query<LandingPageStatistics>(param).Returns(new List<LandingPageStatistics>
            {
                new LandingPageStatistics { Fdm = 1, LanguageCode = "", LanguageName = "", SignUps = 1, SportCode = "", TotalClick = 1, UniqueClick = 1 }
            });
            var repo = CreateRepo();

            var result = repo.GetLandingPageStatistics(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetSStatisticsOverview_WhenCall_QueryStatisticsOverview(bool isInternal)
        {
            const string uSpkey = "Aff_User_StatisticsSel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpkey + dbMode;
            var param = new StatisticsOverviewParam
            {
                AffId = 1,
                IsInternal = isInternal,
                MonthDate = DateTime.Now
            };
            var db = Builder.Build(uSpkey, dbMode);
            db.QueryEntity<AccountOverviewStatistics>(param).Returns(new AccountOverviewStatistics
            {
                ActivePlayers = 1,
                BalanceBroughtForward = 100,
                Deposit = 100,
                DepositUp = 100,
                NewSignups = 2,
                QualifiedMembers = 1,
                TotalPlayers = 10,
                Turnover = 100,
                TurnoverUp = 100
            });
            var repo = CreateRepo();

            var result = repo.GetStatisticsOverview(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMarketingPageStatistics_WhenCall_QueryLandingPage()
        {
            const string uSpkey = "Aff_User_Rpt_MarketingPage_Statistics";
            var param = new MarketingPageStatisticsParam();
            var db = Builder.Build(uSpkey);
            db.Query<MarketingPageStatistics>(param).Returns(new List<MarketingPageStatistics>
            {
                new MarketingPageStatistics {
                    ChannelName = "",
                    AffMediaLink = "",
                    MarketName = "",
                    TotalClick = 1,
                    Language = "",
                    UniqueClick = 1
                }
            });
            var repo = CreateRepo();

            var result = repo.GetMarketingPageStatistics(param);

            Assert.Equal(uSpkey, db.SettingKey);
            Assert.NotEmpty(result);
        }
    }
}
