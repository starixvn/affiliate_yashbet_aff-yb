using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class MarketingRepository : BaseRepository, IMarketingRepository
    {

        private readonly IObjectDBBuilder _builder;

        public MarketingRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public IEnumerable<CategoryType> GetCategoryList(MediaCategoryParam param, bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaCategory_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<CategoryType>(param);
            }
        }

        public IEnumerable<CurrencyType> GetCurrencyList(bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaCurrency_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<CurrencyType>();
            }
        }

        public IEnumerable<BannerType> GetBannerTypeList(bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaType_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<BannerType>();
            }
        }

        public IEnumerable<SizeType> GetSizeList(SizeParam param, bool isInternal)
        {
            const string uSpKey = "Aff_Admin_MediaSize_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<SizeType>(param);
            }
        }

        public IEnumerable<Domain> GetAvailableDomainList(int affId, bool isMember, bool isInternal)
        {
            var uSpKey = isMember ? "Aff_User_Domain_Sel" : "Aff_User_SubAffiliateDomain_Sel";

            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return isMember ? objectDb.Query<Domain>(new { AffId = affId }) : objectDb.Query<Domain>();
            }
        }

        public IEnumerable<MarketingItem> GetBannerList(MarketingBannerParam bannerParam)
        {
            const string uSpKey = "Aff_Admin_Banner_Sel";

            var dbMode = bannerParam.IsInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<MarketingItem>(bannerParam);
            }
        }

        public IEnumerable<MarketingItem> GetTextList(MarketingTextParam textParam)
        {
            const string uSpKey = "Aff_Admin_Text_Sel";
            var dbMode = textParam.IsInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<MarketingItem>(textParam);
            }
        }

        public IEnumerable<LandingPage> GetLandingPageList(bool isInternal)
        {
            const string SpKey = "Aff_Admin_LandingPage_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var db = _builder.Build(SpKey, dbMode))
            {
                return db.Query<LandingPage>();
            }
        }

        public (int Code, string ErrMsg) AddMarketingChannel(int userId, string channelCode, string channelName)
        {
            const string spKey = "Aff_User_AffMediaChannel_Ins";

            using (var objectDb = _builder.Build(spKey))
            {
                AddOutputParam(objectDb);
                objectDb.ExecuteNonQuery(new { AffId = userId, ChannelCode = channelCode, ChannelName = channelName });
                return GetResult(objectDb);
            }
        }

        public IEnumerable<MarketingChannel> GetMarketingChannels(int userId)
        {
            const string spKey = "Aff_User_AffMediaChannel_Sel";

            using (var objectDb = _builder.Build(spKey))
            {
                return objectDb.Query<MarketingChannel>(new { AffId = userId });
            }
        }

        public (int Code, string ErrMsg) ToggleMarketingLink(MarketingLinkParam param)
        {
            const string spKey = "Aff_User_AffMediaLink_ToggleChannel";

            using (var objectDb = _builder.Build(spKey))
            {
                AddOutputParam(objectDb);
                objectDb.ExecuteNonQuery(param);
                return GetResult(objectDb);
            }
        }

        public IEnumerable<MarketingChannelLink> GetUsingMarketingLink(int userId)
        {
            const string spKey = "Aff_User_AffMediaChannel_Statistics";

            using (var objectDb = _builder.Build(spKey))
            {
                return objectDb.Query<MarketingChannelLink>(new { AffId = userId });
            }
        }

    }
}