using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository.UserPanel
{
    public class MarketingRepositoryTest : RepositoryTestBase<MarketingRepository>
    {
        private const int AffiliateLink = 1;
        private const int MediaGallery = 2;

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetCategoryList_WhenCall_QueryCategory(bool isMediaPage, bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaCategory_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var typeId = isMediaPage ? AffiliateLink : MediaGallery;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<CategoryType>().ReturnsForAnyArgs(new List<CategoryType> { new CategoryType() });

            var param = new MediaCategoryParam
            {
                TargetId = TargetSite.TargetToMember,
                TypeId = typeId
            };

            var result = repo.GetCategoryList(param, isInternal);

            Assert.Equal(key, db.SettingKey);
            db.Received().Query<CategoryType>(Arg.Is<MediaCategoryParam>(a => CheckPropertyValue(a, "TypeId", typeId)));
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetCurrencyList_WhenCall_QueryCurrencyType(bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaCurrency_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<CurrencyType>().ReturnsForAnyArgs(new List<CurrencyType> { new CurrencyType() });

            var result = repo.GetCurrencyList(isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBannerTypeList_WhenCall_QueryBannerType(bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaType_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<BannerType>().Returns(new List<BannerType> { new BannerType() });

            var result = repo.GetBannerTypeList(isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetSizeList_WhenCall_QuerySizeType(bool isInternal)
        {
            var param = new SizeParam { };
            const string uSpKey = "Aff_Admin_MediaSize_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<SizeType>(param).Returns(new List<SizeType> { new SizeType { } });

            var result = repo.GetSizeList(param, isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDomainList_WhenCallWithIsMember_QueryDomain(bool isInternal)
        {
            const string uSpKey = "Aff_User_Domain_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetAvailableDomainList(1, true, isInternal);

            Assert.Equal(key, db.SettingKey);
            db.Received().Query<Domain>(Arg.Compat.Is<object>(o => CheckPropertyValue(o, "AffId", 1)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDomainList_WhenCallWithIsSubAff_QueryDomain(bool isInternal)
        {
            const string uSpKey = "Aff_User_SubAffiliateDomain_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(null, null);

            repo.GetAvailableDomainList(1, false, isInternal);

            Assert.Equal(key, db.SettingKey);
            db.Received().Query<Domain>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBannerList_WhenCall_QueryBanner(bool isInternal)
        {
            var param = new MarketingBannerParam();
            param.IsInternal = isInternal;
            const string uSpKey = "Aff_Admin_Banner_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<MarketingItem>(param).Returns(new List<MarketingItem> { new MarketingItem() });

            var result = repo.GetBannerList(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetTextList_WhenCall_QueryTextList(bool isInternal)
        {
            var param = new MarketingTextParam();
            param.IsInternal = isInternal;
            const string uSpKey = "Aff_Admin_Text_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = uSpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(uSpKey, dbMode);
            db.Query<MarketingItem>(param).Returns(new List<MarketingItem> { new MarketingItem() });

            var result = repo.GetTextList(param);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLandingPageList_WhenCall_QueryLandingPage(bool isInternal)
        {
            const string SpKey = "Aff_Admin_LandingPage_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;
            var key = SpKey + dbMode;
            var repo = CreateRepo();
            var db = Builder.Build(SpKey, dbMode);
            db.Query<LandingPage>().Returns(new List<LandingPage> { new LandingPage() });

            var result = repo.GetLandingPageList(isInternal);

            Assert.Equal(key, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void AddMarketingChannel_WhenCall_AddSuccessfully()
        {
            const string SpKey = "Aff_User_AffMediaChannel_Ins";
            var repo = CreateRepo();
            var db = Builder.Build(SpKey);
            db.When(d => d.ExecuteNonQuery(Arg.Any<object>()))
                .Do(c => (c.Args()[0]).ReturnsForAnyArgs(0));

            var (Code, ErrMsg) = repo.AddMarketingChannel(123, "abc", "Abc");

            Assert.Equal(SpKey, db.SettingKey);
            Assert.Equal(0, Code);
        }

        [Fact]
        public void GetMarketingChannels_WhenCall_QueryMarketingChannels()
        {
            const string SpKey = "Aff_User_AffMediaChannel_Sel";
            var repo = CreateRepo();
            var db = Builder.Build(SpKey);
            db.Query<MarketingChannel>(Arg.Any<object>()).Returns(new List<MarketingChannel> { new MarketingChannel() });

            var result = repo.GetMarketingChannels(123);

            Assert.Equal(SpKey, db.SettingKey);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToggleMarketingLink_WhenCall_ToggleSuccessfully()
        {
            const string SpKey = "Aff_User_AffMediaLink_ToggleChannel";
            var repo = CreateRepo();
            var db = Builder.Build(SpKey);
            db.When(d => d.ExecuteNonQuery(Arg.Any<object>()))
                .Do(c => (c.Args()[0]).ReturnsForAnyArgs(0));

            var (Code, ErrMsg) = repo.ToggleMarketingLink(new MarketingLinkParam());

            Assert.Equal(SpKey, db.SettingKey);
            Assert.Equal(0, Code);
        }

        [Fact]
        public void GetUsingMarketingLink_WhenCall_QueryUsingMarketingLink()
        {
            const string SpKey = "Aff_User_AffMediaChannel_Statistics";
            var repo = CreateRepo();
            var db = Builder.Build(SpKey);
            db.Query<MarketingChannelLink>(Arg.Any<object>()).Returns(new List<MarketingChannelLink> { new MarketingChannelLink() });

            var result = repo.GetUsingMarketingLink(123);

            Assert.Equal(SpKey, db.SettingKey);
            Assert.NotEmpty(result);
        }
    }
}
