using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IMarketingService
    {
        IEnumerable<CategoryType> GetCategoryList(bool isMediaPage, AffiliateLinkType linkType, bool isInternal = false);

        IEnumerable<LanguageType> GetLanguageList(bool isInternal);

        IEnumerable<CurrencyType> GetCurrencyList(bool isInternal);

        IEnumerable<BannerType> GetBannerTypeList(bool isInternal = false);

        IEnumerable<SizeType> GetSizeList(SizeParam param, bool isInternal = false);

        IEnumerable<Domain> GetAvailableDomainList(int affId, AffiliateLinkType linkType, bool isInternal = false);

        IDictionary<string, LanguageLinkViewModel> GenerateLinkList(LinkGeneratingParam linkGeneratedParam);

        IEnumerable<MarketingItem> GetBannerList(MarketingBannerParam bannerParam);

        IEnumerable<MarketingItem> GetTextList(MarketingTextParam textParam);

        IEnumerable<LandingPage> GetLandingPageList(bool isInternal = false, AffiliateLinkType linkType = AffiliateLinkType.AffiliateLink);

        ResultInfo<MarketingChannel> AddMarketingChannel(int userId, string channelName);

        IDictionary<string, string> GetChannels(int userId);

        string GetTextLinkTemplate(AffiliateLinkType linkType);

        ResultInfo ToggleMarketingLink(MarketingLinkParam param);

        IEnumerable<IGrouping<string, MarketingChannelLink>> GetUsingMarketingLinks(int userId);
    }
}
