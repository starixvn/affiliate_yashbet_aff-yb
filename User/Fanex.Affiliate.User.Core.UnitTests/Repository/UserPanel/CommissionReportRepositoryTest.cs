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
    public class CommissionReportRepositoryTest : RepositoryTestBase<CommissionReportRepository>
    {
        [Fact]
        public void GetAffiliateCommission_WhenCall_QueryAffiliateCommissions()
        {
            const string uSpKey = "Aff_User_Rpt_Commission";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);
            db.Query<AffiliateCommission>().ReturnsForAnyArgs(new List<AffiliateCommission>
            {
                new AffiliateCommission
                {
                    Bonus = 100, CalculatedType =1, Commission = 100, CommissionRate =100,
                    MonthDate = DateTime.Now, PlatformFee = 100, ProductId = 1, ProductName = "saba", TurnOver = 100, GGR = 100
                }
            });

            var param = new CommissionReportParam
            {
                AffId = 1,
                BaseCurrency = 20,
                WinLostDate = DateTime.Now
            };

            var result = repo.GetAffiliateCommission(param);

            Assert.Equal(uSpKey, db.SettingKey);
            Assert.Equal(100, result.FirstOrDefault().TotalCommission);
        }
    }
}
