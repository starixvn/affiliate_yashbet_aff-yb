using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class CurrencyRepositoryTest : RepositoryTestBase<CurrencyRepository>
    {
        [Fact]
        public void GetBaseCurrencyList_WhenCall_CallQuery()
        {
            const string uSpKey = "Aff_BaseCurrency_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetBaseCurrencyList();

            Assert.Equal(uSpKey, db.SettingKey);
            db.Received().Query<Currency>();
        }
    }
}
