using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class StatementHistoryRepositoryTest : RepositoryTestBase<StatementHistoryRepository>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStatementHistory_WhenCall_QueryStatementHistory(bool isInternal)
        {
            const string uSpKey = "Aff_User_StatementHistory_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var param = new StatementHistoryParam
            {
                IsInternal = isInternal
            };
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<StatementHistory>(param).Returns(new List<StatementHistory> { new StatementHistory {
                TransType = 123,
                Amount = 100,
                CalculatedType = 1,
                Description = string.Empty,
                MonthDate = new DateTime(2020, 12, 01),
                Status = "Payment"
            } });
            var repo = CreateRepo();

            var result = repo.GetStatementHistory(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetProductList_WhenCall_QueryProduct(bool isInternal)
        {
            const string uSpKey = "Aff_ProductList_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<ProductItem>().Returns(new List<ProductItem> { new ProductItem { } });
            var repo = CreateRepo();

            var result = repo.GetProductList(isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetDailyEarning_WhenCall_ReturnDailyEarningResult(bool isInternal, bool isNextResult)
        {
            const string uSpKey = "Aff_DailyEarning_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var param = new DailyEarningParam { IsInternal = isInternal };
            var db = Builder.Build(uSpKey, dbMode);

            var reader = db.ExecuteReader(It.IsAny<object>());
            reader.Read().Returns(true, false, true);
            reader.NextResult().Returns(isNextResult, true, false);
            reader["Deposit"].Returns(100);
            reader["Withdrawal"].Returns(100);
            reader["GGR"].Returns(100);
            reader["Bonus"].Returns(100);
            reader["Rebate"].Returns(10);
            reader["WinlostDate"].Returns(new DateTime(2021, 10, 25));

            var repo = CreateRepo();

            var result = repo.GetDailyEarning(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result.MainDataList);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetSubAffCommission_WhenCall_ReturnSubAffCommission(bool isInternal)
        {
            const string uSpKey = "Aff_SubAffiliate_Commission_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var param = new SubAffComParam
            {
                AffId = 123,
                MonthDate = new DateTime(2020, 12, 01),
                BaseCurrency = 1,
                IsInternal = isInternal
            };
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<SubAffComInfo>(param).Returns(new List<SubAffComInfo> { new SubAffComInfo {
                AffId = 123,
                UserName = "Nova",
                Commission = 100,
                SubAffCommission = 0
            } });

            var result = repo.GetSubAffCommission(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }
    }
}
