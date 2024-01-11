using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    [CustomAuthorize]
    public class BaseMarketingController : BaseController
    {
        protected readonly IMarketingService MarketingService;
        protected readonly IAppSetting AppSetting;

        public BaseMarketingController(ISessionService sessionService,
            IAppSetting appSetting,
            ILanguageService languageService,
            IMarketingService marketingService)
            : base(sessionService, languageService)
        {
            MarketingService = marketingService;
            AppSetting = appSetting;
        }

        protected IDictionary<string, LanguageLinkViewModel> GetLinkForMemberList(QrCodeParam param)
        {
            var selectedDomain = GetDomain(param.SelectedDomainId, param.SelectedLinkType);

            if (selectedDomain == null)
            {
                return new Dictionary<string, LanguageLinkViewModel>();
            }

            var userSession = SessionService.UserSession;

            var linkGenerationParam = new LinkGeneratingParam
            {
                AffId = userSession.AffId,
                AffCode = userSession.AffCode,
                TextLinkTemplate = AppSetting.TextLinkTemplate,
                PreferredLanguage = userSession.PreferredLanguage,
                SelectedLanguage = param.SelectedLanguage,
                SelectedDomain = selectedDomain,
                SelectedPage = param.SelectedPage,
                SelectedChannel = param.SelectedChannel,
            };

            return MarketingService.GenerateLinkList(linkGenerationParam);
        }

        protected IDictionary<string, LanguageLinkViewModel> GetLinkForSubAffList(QrCodeParam param)
        {
            var selectedDomain = GetDomain(param.SelectedDomainId, param.SelectedLinkType);

            if (selectedDomain == null)
            {
                return new Dictionary<string, LanguageLinkViewModel>();
            }

            var userSession = SessionService.UserSession;

            var linkGenerationParam = new LinkGeneratingParam
            {
                AffId = userSession.AffId,
                AffCode = userSession.AffCode,
                TextLinkTemplate = AppSetting.SubAffTextLinkTemplate,
                PreferredLanguage = userSession.PreferredLanguage,
                SelectedLanguage = param.SelectedLanguage,
                SelectedDomain = selectedDomain,
                SelectedPage = param.SelectedPage,
                SelectedChannel = param.SelectedChannel
            };

            return MarketingService.GenerateLinkList(linkGenerationParam);
        }

        protected RefLinkViewModel GetAffLinkTypeViewModel()
        {
            var affId = SessionService.UserSession.AffId;
            var domains = MarketingService.GetAvailableDomainList(affId, AffiliateLinkType.AffiliateLink, SessionService.IsInternal);
            var pages = MarketingService.GetLandingPageList(SessionService.IsInternal);

            return new RefLinkViewModel
            {
                AffLinkType = AffiliateLinkType.AffiliateLink,
                AffTypeDisplayName = Lang.AffilateLink,
                Domains = domains,
                SitePages = pages
            };
        }

        protected RefLinkViewModel GetSubAffLinkTypeViewModel()
        {
            var affId = SessionService.UserSession.AffId;
            var domains = MarketingService.GetAvailableDomainList(affId, AffiliateLinkType.SubAffiliateLink, SessionService.IsInternal);
            var pages = MarketingService.GetLandingPageList(SessionService.IsInternal, AffiliateLinkType.SubAffiliateLink);

            return new RefLinkViewModel
            {
                AffLinkType = AffiliateLinkType.SubAffiliateLink,
                AffTypeDisplayName = Lang.SubAffiliateLink,
                Domains = domains,
                SitePages = pages
            };
        }

        private Domain GetDomain(int? domainId, AffiliateLinkType linkType)
            => MarketingService
            .GetAvailableDomainList(SessionService.UserSession.AffId, linkType, SessionService.IsInternal)
            .FirstOrDefault(d => d.DomainId == domainId);
    }
}
