using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.Extensions;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class MarketingService : IMarketingService
    {
        private readonly IMarketingRepository _marketingRepository;
        private readonly IMemoryCacheService _cacheService;
        private readonly IAppSetting _appSetting;
        private readonly ILanguageService _languageService;

        private const int AffiliateLink = 1;
        private const int MediaGallery = 2;

        public MarketingService(IMarketingRepository marketingRepository, IMemoryCacheService cacheService, IAppSetting appSetting, ILanguageService languageService)
        {
            _marketingRepository = marketingRepository;
            _cacheService = cacheService;
            _appSetting = appSetting;
            _languageService = languageService;
        }

        public IEnumerable<CategoryType> GetCategoryList(bool isMediaPage, AffiliateLinkType linkType, bool isInternal = false)
        {
            var cacheKey = isMediaPage ? CacheKey.BannerCategoryList : CacheKey.TextCategoryList;
            cacheKey = $"{cacheKey}-{linkType}";

            var categoryList = _cacheService.Get<IEnumerable<CategoryType>>(cacheKey);
            if (categoryList == null)
            {
                var mediaType = isMediaPage ? AffiliateLink : MediaGallery;
                var target = linkType;
                var param = new MediaCategoryParam
                {
                    TargetId = (int)target,
                    TypeId = mediaType
                };
                categoryList = _marketingRepository.GetCategoryList(param, isInternal);

                _cacheService.Set(cacheKey, categoryList);
            }

            return categoryList ?? new List<CategoryType>();
        }

        public IEnumerable<LanguageType> GetLanguageList(bool isInternal)
            => _languageService.GetLanguageList(isInternal)
            .Select(l => new LanguageType
            {
                DisplayName = l.DisplayName,
                MarketId = l.Code,
                MarketName = l.EnglishName,
                MarketNativeName = l.NativeName,
                ShortedCode = l.ShortedCode
            });

        public IEnumerable<CurrencyType> GetCurrencyList(bool isInternal)
        {
            var currencyList = _cacheService.Get<IEnumerable<CurrencyType>>(CacheKey.CurrencyList);

            if (currencyList == null)
            {
                currencyList = _marketingRepository.GetCurrencyList(isInternal);
                _cacheService.Set(CacheKey.CurrencyList, currencyList);
            }

            return currencyList ?? new List<CurrencyType>();
        }

        public IEnumerable<SizeType> GetSizeList(SizeParam param, bool isInternal = false)
        {
            return _marketingRepository.GetSizeList(param, isInternal);
        }

        public IEnumerable<BannerType> GetBannerTypeList(bool isInternal = false)
        {
            var bannerTypeList = _cacheService.Get<IEnumerable<BannerType>>(CacheKey.BannerTypeList);

            if (bannerTypeList == null)
            {
                bannerTypeList = _marketingRepository.GetBannerTypeList(isInternal);
                _cacheService.Set(CacheKey.BannerTypeList, bannerTypeList);
            }

            return bannerTypeList ?? new List<BannerType>();
        }

        public IEnumerable<Domain> GetAvailableDomainList(int affId, AffiliateLinkType linkType, bool isInternal = false)
        {
            var isMember = linkType == AffiliateLinkType.AffiliateLink;
            var cacheKey = isMember ? CacheKey.DomainList : CacheKey.SubDomainList;

            var domainList = _cacheService.Get<IEnumerable<Domain>>(cacheKey);

            if (domainList == null)
            {
                domainList = _marketingRepository.GetAvailableDomainList(affId, isMember, isInternal);
                _cacheService.Set(cacheKey, domainList, Number.Ten);
            }

            return domainList ?? new List<Domain>();
        }

        public IDictionary<string, LanguageLinkViewModel> GenerateLinkList(LinkGeneratingParam linkGeneratedParam)
        {
            var languages = GetLanguageUsedForGeneration(linkGeneratedParam);
            return GenerateLink(languages, linkGeneratedParam);
        }

        public IEnumerable<MarketingItem> GetBannerList(MarketingBannerParam bannerParam)
        {
            const int LengthOfSplittingSize = 2;
            var bannerList = _marketingRepository.GetBannerList(bannerParam);

            foreach (var banner in bannerList)
            {
                banner.FileName = bannerParam.BannerDomain + banner.FileName;
                banner.SelectedDomainName = bannerParam.SelectedDomain.DomainName;

                var sizes = banner.Size.Split('x');
                if (sizes.Count() == LengthOfSplittingSize)
                {
                    banner.Width = Convert.ToInt32(sizes[0]);
                    banner.Height = Convert.ToInt32(sizes[1]);
                }

                var language = _languageService.GetLanguageList().FirstOrDefault(l => l.EnglishName.MatchWith(banner.Language));

                if (language != null)
                {
                    banner.RedirectLink = GenerateLink(bannerParam, language.ShortedCode);
                }
            }

            return bannerList;
        }

        public IEnumerable<MarketingItem> GetTextList(MarketingTextParam textParam)
        {
            var textList = _marketingRepository.GetTextList(textParam);

            foreach (var text in textList)
            {
                text.Message = HttpContext.Current.Server.HtmlDecode(text.Message.Replace("\r\n", "<br/>").Replace("\n", "<br/>"));
                text.ScriptSrc = $"{textParam.ScriptUrl}?textId={text.TextId}&targetId={textParam.TargetId}";
                var language = _languageService.GetLanguageList().FirstOrDefault(l => l.EnglishName.MatchWith(text.Language));

                if (language != null)
                {
                    text.RedirectLink = GenerateLink(textParam, language.ShortedCode);
                }
            }

            return textList;
        }

        public IEnumerable<LandingPage> GetLandingPageList(bool isInternal = false, AffiliateLinkType linkType = AffiliateLinkType.AffiliateLink)
        {
            var isTargetToMember = linkType == AffiliateLinkType.AffiliateLink;
            return isTargetToMember
                ? _marketingRepository.GetLandingPageList(isInternal)
                : GetSubAffLandingPages();
        }

        public string GetTextLinkTemplate(AffiliateLinkType linkType)
        {
            switch (linkType)
            {
                case AffiliateLinkType.AffiliateLink:
                    return _appSetting.TextLinkTemplate;

                case AffiliateLinkType.SubAffiliateLink:
                    return _appSetting.SubAffTextLinkTemplate;

                default:
                    return _appSetting.TextLinkTemplate;
            }
        }

        public ResultInfo<MarketingChannel> AddMarketingChannel(int userId, string channelName)
        {
            var channelCode = NormalizeName(channelName);
            var (Code, ErrMsg) = _marketingRepository.AddMarketingChannel(userId, channelCode, channelName);
            var message = GenAddChannelErrMsg(Code);

            return new ResultInfo<MarketingChannel>
            {
                IsOk = Code == 0,
                Message = string.IsNullOrWhiteSpace(message) ? ErrMsg : message,
                Data = new MarketingChannel { ChannelCode = channelCode, ChannelName = channelName }
            };
        }

        public IDictionary<string, string> GetChannels(int userId)
            => _marketingRepository.GetMarketingChannels(userId).ToDictionary(c => c.ChannelCode, c => c.ChannelName);

        public ResultInfo ToggleMarketingLink(MarketingLinkParam param)
        {
            var (Code, ErrMsg) = _marketingRepository.ToggleMarketingLink(param);
            var message = GenToggleErrMsg(Code);

            return new ResultInfo
            {
                IsOk = Code == 0,
                Message = string.IsNullOrWhiteSpace(message) ? ErrMsg : message
            };
        }

        public IEnumerable<IGrouping<string, MarketingChannelLink>> GetUsingMarketingLinks(int userId)
            => _marketingRepository.GetUsingMarketingLink(userId).GroupBy(c => c.ChannelCode);

        private Dictionary<string, Language> GetLanguageUsedForGeneration(LinkGeneratingParam linkGeneratedParam)
        {
            var result = new Dictionary<string, Language>();
            var languageList = _languageService.GetLanguageList();

            if (string.IsNullOrEmpty(linkGeneratedParam.SelectedLanguage))
            {
                foreach (var language in languageList)
                {
                    result.Add(language.Code, language);
                }
            }
            else
            {
                var language = languageList.FirstOrDefault(l => l.Code.MatchWith(linkGeneratedParam.SelectedLanguage));
                if (language != null)
                {
                    result.Add(language.Code, language);
                }
            }

            return result;
        }

        private static IDictionary<string, LanguageLinkViewModel> GenerateLink(Dictionary<string, Language> languages, LinkGeneratingParam param)
        {
            var linkDict = new Dictionary<string, LanguageLinkViewModel>();

            if (languages != null)
            {
                foreach (var l in languages.Values)
                {
                    var linkViewModel = new LanguageLinkViewModel
                    {
                        Language = l,
                        Link = GenerateLink(param, l.ShortedCode)
                    };

                    linkDict.Add(l.NativeName, linkViewModel);
                }
            }

            return linkDict;
        }

        private static string GenerateLink(LinkBaseParam linkParam, string shortedLanguageCode)
        {
            var selectedDomain = linkParam.SelectedDomain;
            var template = linkParam.TextLinkTemplate;
            var scheme = selectedDomain.IsSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

            var result = template.Replace("{scheme}", scheme)
                                .Replace("{domain}", selectedDomain.DomainName)
                                .Replace("{custid}", linkParam.AffCode.ToString())
                                .Replace("{target}", HttpUtility.UrlPathEncode(linkParam.SelectedPage))
                                .Replace("{lang}", shortedLanguageCode)
                                .Replace("{channel}", linkParam.SelectedChannel);

            return result;
        }

        private IEnumerable<LandingPage> GetSubAffLandingPages()
        {
            return new List<LandingPage>
            {
                new LandingPage{PageCode = "Signup", PageName="SignUp"}
            };
        }

        private static string GenAddChannelErrMsg(int code)
        {
            switch (code)
            {
                case ErrCodes.OneHundredOne:
                    return Error.ErrInValidAff;

                case ErrCodes.OneHundredTwo:
                    return Error.ErrChannelExisted;

                case ErrCodes.OneHundredThree:
                    return Error.ErrInternalError;

                default:
                    return string.Empty;
            }
        }

        private static string GenToggleErrMsg(int code)
        {
            switch (code)
            {
                case ErrCodes.OneHundredOne:
                    return Error.ErrInValidAff;

                case ErrCodes.OneHundredTwo:
                    return Error.ErrInternalError;

                default:
                    return string.Empty;
            }
        }

        private static string NormalizeName(string channelName)
        {
            return channelName.Replace(" ", String.Empty).ToLowerInvariant();
        }
    }
}