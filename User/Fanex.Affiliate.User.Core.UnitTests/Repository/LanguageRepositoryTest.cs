using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.Repository;
using NSubstitute;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository
{
    public class LanguageRepositoryTest : RepositoryTestBase<LanguageRepository>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLanguageList_WhenCall_DbQuery(bool isInternal)
        {
            var prefixKey = "Aff_Market_Sel";
            var sufix = isInternal ? DBMode.View : DBMode.Main;
            var key = prefixKey + sufix;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetLanguageList(isInternal);

            Assert.Equal(key, db.SettingKey);
            db.Received().Query<LanguageType>();
        }
    }
}
