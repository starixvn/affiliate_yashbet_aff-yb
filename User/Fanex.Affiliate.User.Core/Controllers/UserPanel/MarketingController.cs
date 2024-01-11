namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Constants;
    using DTO.UserPanel;
    using Extensions;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.DTO;
    using Fanex.Affiliate.User.Core.Enum;
    using Fanex.Affiliate.User.Core.MetalModels;
    using Fanex.Affiliate.User.Core.Resources;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using ViewModels.UserPanel.Marketing;

    [CustomAuthorize]
    public class MarketingController : BaseMarketingController
    {
        private readonly IQrCodeGeneratorService QrCodeGeneratorService;
        private readonly IFileSystem FileSystem;

        public MarketingController(ISessionService sessionService, IAppSetting appSetting,
            ILanguageService languageService,
            IMarketingService marketingService,
            IQrCodeGeneratorService qrCodeGeneratorService,
            IFileSystem fileSystem) : base(sessionService, appSetting, languageService, marketingService)
        {
            QrCodeGeneratorService = qrCodeGeneratorService;
            FileSystem = fileSystem;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSubCategory(string category, AffiliateLinkType linkType)
        {
            var subCategoryList = MarketingService.GetCategoryList(true, linkType, SessionService.IsInternal)
                .Where(c => c.Category.MatchWith(category) && !string.IsNullOrEmpty(c.SubCategory));
            var newSub = subCategoryList.Select(i => new SelectListItem
            {
                Value = i.SubCategory,
                Text = Lang.ResourceManager.GetString(Regex.Replace(i.SubCategory, @"\s+", "")) ?? i.SubCategory
            });

            return Json(newSub,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSize(SizeParam param)
        {
            param.AffType = SessionService.UserSession.AffType;
            param.Language = GetEnglishName(param.Language);
            var sizeList = MarketingService.GetSizeList(param, SessionService.IsInternal);

            return Json(sizeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetBanner(MarketingBannerParam bannerParam)
        {
            var userSession = SessionService.UserSession;
            var selectedDomain = MarketingService.GetAvailableDomainList(userSession.AffId, bannerParam.TargetId, SessionService.IsInternal)
                                                    .FirstOrDefault(d => d.DomainId == bannerParam.SelectedDomainId);

            var bannerSettingKey = userSession.PreferredLanguage.Replace("-", string.Empty).ToUpperInvariant();

            bannerParam.AffId = userSession.AffId;
            bannerParam.AffCode = userSession.AffCode;
            bannerParam.AffType = userSession.AffType;
            bannerParam.SelectedDomain = selectedDomain;
            bannerParam.TextLinkTemplate = MarketingService.GetTextLinkTemplate(bannerParam.TargetId);
            bannerParam.BannerDomain = AppSetting.GetValue($"BannerDomain{bannerSettingKey}") ??
                AppSetting.BannerDomain;
            bannerParam.Language = GetEnglishName(bannerParam.Language);

            var bannerList = GetBannerList(bannerParam);
            ViewBag.TotalItem = bannerParam.Nrows;

            return View("~/Views/UserPanel/AccountOverview/Partial/_BannerList.cshtml", bannerList);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult RenderQrCodePage(QrCodeParam param)
        {
            IDictionary<string, QrCodeLinkViewModel> result = new Dictionary<string, QrCodeLinkViewModel>();

            if (param.SelectedDomainId != null)
            {
                result = GenerateQrCodeViewModel(param);
                var usingLinks = MarketingService.GetUsingMarketingLinks(SessionService.UserSession.AffId)
                    .FirstOrDefault(l => l.Key == param.SelectedChannel);

                if (usingLinks != null && result != null)
                {
                    foreach (var q in result.Values)
                    {
                        q.IsShow = usingLinks.Any(l => l.AffMediaLink.Equals(q.Link, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }

            ViewBag.SelectedDomainId = param.SelectedDomainId;
            ViewBag.SelectedPage = param.SelectedPage;
            ViewBag.SelectedLinkType = param.SelectedLinkType;
            ViewBag.SelectedChannel = param.SelectedChannel;
            ViewBag.Language = param.SelectedLanguage;

            return View("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result?.Values);
        }

        [HttpGet]
        public ActionResult DownloadQrCode(QrCodeParam param)
        {
            var qrCodes = GenerateQrCodeViewModel(param, false);
            if (qrCodes.Count == 0)
            {
                return HttpNotFound();
            }
            var qrCode = qrCodes.First();
            var imageFile = Convert.FromBase64String(qrCode.Value.QrCode);
            return File(imageFile, "image/png", $"{param.SelectedLanguage}.png");
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult AddChannel(string channelName)
        {
            if (channelName.Length > Number.Twenty)
            {
                return Json(new ResultInfo
                {
                    IsOk = false,
                    Message = Error.NameToLong20
                });
            }
            var userId = SessionService.UserSession.AffId;
            return Json(MarketingService.AddMarketingChannel(userId, channelName));
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetChannels()
        {
            var userId = SessionService.UserSession.AffId;
            return Json(MarketingService.GetChannels(userId));
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult ToggleMarketingLink(MarketingLinkViewModel model)
        {
            var userId = SessionService.UserSession.AffId;
            var param = new MarketingLinkParam
            {
                AffId = userId,
                AffMediaLink = model.AffRefLink,
                ChannelCode = model.ChannelCode,
                LanguageCode = model.LanguageCode,
                IsOn = model.IsOn
            };
            var result = MarketingService.ToggleMarketingLink(param);

            return Json(result);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult RenderUsingMarketLinkPage()
        {
            var userId = SessionService.UserSession.AffId;

            return View("~/Views/UserPanel/AccountOverview/Partial/_ChannelLinkStatistic.cshtml", MarketingService.GetUsingMarketingLinks(userId));
        }

        private IEnumerable<MarketingItem> GetBannerList(MarketingBannerParam bannerParam)
        {
            if (bannerParam.SelectedDomain == null)
            {
                return new List<MarketingItem>();
            }
            return MarketingService.GetBannerList(bannerParam);
        }

        private IDictionary<string, QrCodeLinkViewModel> GenerateQrCodeViewModel(QrCodeParam param, bool includePrefix = true)
        {
            var selectedLInkType = param.SelectedLinkType;

            var linkList = selectedLInkType == Core.Enum.AffiliateLinkType.AffiliateLink
                ? GetLinkForMemberList(param)
                : GetLinkForSubAffList(param);

            return GetQrCodes(linkList, includePrefix);
        }

        private IDictionary<string, QrCodeLinkViewModel> GetQrCodes(IDictionary<string, LanguageLinkViewModel> txtLinkList, bool includePrefix)
        {
            var qrCodeLinks = txtLinkList.Select(link => GetQrCodeParam(link, includePrefix));
            return qrCodeLinks.ToDictionary(itm => itm.DisplayName);
        }

        private QrCodeLinkViewModel GetQrCodeParam(KeyValuePair<string, LanguageLinkViewModel> link, bool includePrefix)
        {
            var icon = GetQrCodeIcon();
            var language = link.Value.Language;
            var textLink = link.Value.Link;
            var qrCode = QrCodeGeneratorService.GenerateBase64QrCode(textLink, icon, includePrefix);
            return new QrCodeLinkViewModel
            {
                Language = language,
                Link = textLink,
                QrCode = qrCode,
                DisplayName = link.Key
            };
        }

        private Image GetQrCodeIcon()
        {
            var logoPath = HttpContext.Server.MapPath($"~/www/build/styles/Images/themes/logo.png");
            var qrLogoPath = HttpContext.Server.MapPath($"~/www/build/styles/Images/themes/logo-qrcode.png");
            var path = FileSystem.Exists(qrLogoPath) ? qrLogoPath : logoPath;

            return QrCodeGeneratorService.GetImage(path);
        }

        private string GetEnglishName(string languageCode)
        {
            return LanguageService.GetLanguageList().FirstOrDefault(l => l.Code.MatchWith(languageCode))?.EnglishName;
        }
    }
}