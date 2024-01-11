using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers.UserPanel;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.Extensions;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel.Marketing;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class MarketingControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly IAppSetting AppSetting;
        private readonly ILanguageService LanguageService;
        private readonly IMarketingService MarketingService;
        private readonly IQrCodeGeneratorService QrCodeService;
        private readonly IFileSystem FileSystem;

        private static IEnumerable<MarketingItem> MarketingItems => new List<MarketingItem>
        {
            new MarketingItem {BannerId =1 },
            new MarketingItem {BannerId  = 2 }
        };

        private const string BASE64_IMAGE = "base64Image";
        private static Image Image => Image.FromStream(new MemoryStream(Convert.FromBase64String(Constant.LogoImageBase64)));

        public MarketingControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            AppSetting = Substitute.For<IAppSetting>();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
            MarketingService = NSubtituteMockingHelper.FakeMarkengtingService();
            QrCodeService = Substitute.For<IQrCodeGeneratorService>();
            FileSystem = Substitute.For<IFileSystem>();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        [InlineData("test2")]
        public void GetSubCategory_WhenCall_ReturnCorrectNumberOfSubCategory(string category)
        {
            var subCate = Constant.CategoryList.Where(c => c.Category.MatchWith(category) && !string.IsNullOrEmpty(c.SubCategory));
            var controller = CreateMarketingController();
            var result = controller.GetSubCategory(category, AffiliateLinkType.AffiliateLink) as JsonResult;
            var model = result.Data as IEnumerable<SelectListItem>;
            Assert.Equal(subCate.Count(), model.Count());
            Assert.Equal(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
        }

        [Fact]
        public void GetSize_WhenCall_ReturnCorrectJson()
        {
            var controller = CreateMarketingController();
            MarketingService.GetSizeList(Arg.Compat.Any<SizeParam>(), Arg.Compat.Any<bool>())
                .Returns(Constant.SizeTypes);
            var param = new SizeParam { Language = Constant.DefaultLanguage };

            var result = controller.GetSize(param) as JsonResult;
            var model = result.Data as IEnumerable<SizeType>;

            Assert.Collection(model, t => Assert.Contains(Constant.SizeTypes, s => s.SizeId == t.SizeId), t => Assert.Contains(Constant.SizeTypes, s => s.SizeId == t.SizeId));
            Assert.Equal(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
        }

        [Fact]
        public void GetSize_WhenCallWithLanguageIsEmpty_ReturnCorrectJson()
        {
            var controller = CreateMarketingController();
            LanguageService.GetLanguageList().Returns(new List<Language>());
            MarketingService.GetSizeList(Arg.Compat.Any<SizeParam>(), Arg.Compat.Any<bool>())
                .Returns(Constant.SizeTypes);
            var param = new SizeParam { Language = Constant.DefaultLanguage };

            var result = controller.GetSize(param) as JsonResult;
            var model = result.Data as IEnumerable<SizeType>;

            Assert.Collection(model, t => Assert.Contains(Constant.SizeTypes, s => s.SizeId == t.SizeId), t => Assert.Contains(Constant.SizeTypes, s => s.SizeId == t.SizeId));
            Assert.Equal(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
        }

        [Fact]
        public void GetBanner_WhenCallWithNoSelectedDomain_ReturnEmptyModelItem()
        {
            var controller = CreateMarketingController();
            MarketingService.GetAvailableDomainList(Arg.Compat.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(new List<Domain>());
            var param = new MarketingBannerParam();

            var result = controller.GetBanner(param) as ViewResult;
            var model = result.Model as IEnumerable<MarketingItem>;

            Assert.Contains("AccountOverview/Partial/_BannerList.cshtml", result.ViewName);
            Assert.Empty(model);
        }

        [Fact]
        public void GetBanner_WhenCall_ReturnCorrectModel()
        {
            var controller = CreateMarketingController();
            var bannerItems = MarketingItems;
            MarketingService.GetBannerList(Arg.Compat.Any<MarketingBannerParam>()).Returns(bannerItems);
            MarketingService.GetAvailableDomainList(Arg.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(Constant.DomainList);
            var domainId = Constant.DomainList.First().DomainId;
            var param = new MarketingBannerParam { SelectedDomainId = domainId, Language = "vi-VN" };

            var result = controller.GetBanner(param) as ViewResult;
            var model = result.Model as IEnumerable<MarketingItem>;

            Assert.Contains("AccountOverview/Partial/_BannerList.cshtml", result.ViewName);
            Assert.NotEmpty(model);
        }

        [Fact]
        public void RenderQrCodePage_WhenCallWithSelectedDomainIdIsNull_ReturnCorrectView()
        {
            var controller = CreateMarketingController();

            var result = controller.RenderQrCodePage(new QrCodeParam()) as ViewResult;
            var model = result.Model as Dictionary<string, QrCodeLinkViewModel>;

            Assert.Null(model);
            Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result.ViewName);
        }

        [Fact]
        public void RenderQrCodePage_WhenCallWithAffiliateLinkButNotHasDomain_ReturnCorrectView()
        {
            var controller = CreateMarketingController();
            MarketingService.GetAvailableDomainList(Arg.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(Constant.DomainList);
            MarketingService.GetUsingMarketingLinks(Arg.Compat.Any<int>()).Returns(new List<MarketingChannelLink> { new MarketingChannelLink { ChannelName = "ABC" } }.GroupBy(i => i.ChannelName));

            var result = controller.RenderQrCodePage(new QrCodeParam { SelectedDomainId = 3, SelectedLinkType = AffiliateLinkType.AffiliateLink }) as ViewResult;
            var model = result.Model as Dictionary<string, QrCodeLinkViewModel>;

            Assert.Null(model);
            Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result.ViewName);
        }

        [Fact]
        public void RenderQrCodePage_WhenCallWithSubAffiliateLinkButNotHasDomain_ReturnCorrectView()
        {
            var controller = CreateMarketingController();
            MarketingService.GetAvailableDomainList(Arg.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(Constant.DomainList);
            MarketingService.GetUsingMarketingLinks(Arg.Compat.Any<int>()).Returns(new List<MarketingChannelLink> { new MarketingChannelLink { ChannelName = "ABC" } }.GroupBy(i => i.ChannelName));

            var result = controller.RenderQrCodePage(new QrCodeParam { SelectedDomainId = 3, SelectedLinkType = AffiliateLinkType.SubAffiliateLink }) as ViewResult;
            var model = result.Model as Dictionary<string, QrCodeLinkViewModel>;

            Assert.Null(model);
            Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result.ViewName);
        }

        [Fact]
        public void RenderQrCodePage_WhenCallWithAffiliateLinkAndSelectedChannel_ReturnCorrectView()
        {
            using (var image = Image)
            {
                var param = new QrCodeParam
                {
                    SelectedDomainId = 1,
                    SelectedLinkType = AffiliateLinkType.AffiliateLink,
                    SelectedChannel = "ABC"
                };
                var controller = CreateMarketingController();
                controller = NSubtituteMockingHelper.FakeController(controller) as MarketingController;
                controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

                AppSetting.SiteName.Returns("YB");
                FileSystem.Exists(Arg.Any<string>()).Returns(true);
                QrCodeService.GetImage(Arg.Compat.Any<string>()).Returns(image);
                QrCodeService.GenerateBase64QrCode(Arg.Compat.Any<string>(), Arg.Compat.Any<Image>(), Arg.Compat.Any<bool>())
                    .Returns(BASE64_IMAGE);
                MarketingService.GetAvailableDomainList(Arg.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(Constant.DomainList);
                MarketingService.GetUsingMarketingLinks(Arg.Compat.Any<int>()).Returns(new List<MarketingChannelLink>
                {
                    new MarketingChannelLink { ChannelName = "ABC", AffMediaLink = "English.com", ChannelCode = "123", MarketName = "English" }
                }.GroupBy(i => i.ChannelName));

                var result = controller.RenderQrCodePage(param) as ViewResult;
                var model = result.Model as IEnumerable<QrCodeLinkViewModel>;

                Assert.NotEmpty(model);
                Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result.ViewName);
            }
        }

        [Fact]
        public void RenderQrCodePage_WhenCallWithSubAffiliateLinkAndSelectedChannel_ReturnCorrectView()
        {
            using (var image = Image)
            {
                var param = new QrCodeParam
                {
                    SelectedDomainId = 1,
                    SelectedLinkType = AffiliateLinkType.SubAffiliateLink,
                    SelectedChannel = "ABC"
                };
                var controller = CreateMarketingController();
                controller = NSubtituteMockingHelper.FakeController(controller) as MarketingController;
                controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

                AppSetting.SiteName.Returns("YB");
                FileSystem.Exists(Arg.Any<string>()).Returns(false);
                QrCodeService.GetImage(Arg.Compat.Any<string>()).Returns(image);
                QrCodeService.GenerateBase64QrCode(Arg.Compat.Any<string>(), Arg.Compat.Any<Image>(), Arg.Compat.Any<bool>())
                    .Returns(BASE64_IMAGE);
                MarketingService.GetAvailableDomainList(Arg.Any<int>(), Arg.Any<AffiliateLinkType>(), Arg.Any<bool>()).Returns(Constant.DomainList);
                MarketingService.GetUsingMarketingLinks(Arg.Compat.Any<int>()).Returns(new List<MarketingChannelLink>
                {
                    new MarketingChannelLink { ChannelName = "ABC", AffMediaLink = "English.com" }
                }.GroupBy(i => i.ChannelName));

                var result = controller.RenderQrCodePage(param) as ViewResult;
                var model = result.Model as IEnumerable<QrCodeLinkViewModel>;

                Assert.NotEmpty(model);
                Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_GeneratedQrCode.cshtml", result.ViewName);
            }
        }

        [Fact]
        public void DownloadQrCode_WhenCall_ReturnHttpNotFound()
        {
            var param = new QrCodeParam
            {
                SelectedDomainId = 123,
                SelectedLinkType = AffiliateLinkType.AffiliateLink
            };
            var controller = CreateMarketingController();

            var result = controller.DownloadQrCode(param) as HttpNotFoundResult;

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void DownloadQrCode_WhenCall_ReturnFile()
        {
            using (var image = Image)
            {
                var param = new QrCodeParam
                {
                    SelectedDomainId = 1,
                    SelectedLinkType = AffiliateLinkType.AffiliateLink,
                    SelectedChannel = "ABC",
                    SelectedLanguage = "English"
                };
                var controller = CreateMarketingController();
                controller = NSubtituteMockingHelper.FakeController(controller) as MarketingController;
                controller.HttpContext.Server.MapPath(Arg.Compat.Any<string>()).Returns(String.Empty);

                AppSetting.SiteName.Returns("YB");
                FileSystem.Exists(Arg.Any<string>()).Returns(false);
                QrCodeService.GetImage(Arg.Compat.Any<string>()).Returns(image);
                QrCodeService.GenerateBase64QrCode(Arg.Compat.Any<string>(), Arg.Compat.Any<Image>(), Arg.Compat.Any<bool>())
                    .Returns(Constant.LogoImageBase64);

                var result = controller.DownloadQrCode(param) as FileContentResult;

                Assert.Equal("English.png", result.FileDownloadName);
            }
        }

        [Fact]
        public void AddChannel_WhenCallWithInvalidLength_ReturnError()
        {
            var controller = CreateMarketingController();

            var result = controller.AddChannel("123ABC123ABC123ABC123ABC") as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.False(data.IsOk);
        }

        [Fact]
        public void AddChannel_WhenCall_AddSuccessfully()
        {
            MarketingService.AddMarketingChannel(Arg.Any<int>(), Arg.Any<string>()).Returns(new ResultInfo<MarketingChannel> { IsOk = true });
            var controller = CreateMarketingController();

            var result = controller.AddChannel("ABC") as JsonResult;
            var data = result.Data as ResultInfo<MarketingChannel>;

            Assert.True(data.IsOk);
        }

        [Fact]
        public void GetChannels_WhenCall_ReturnCorrectChannels()
        {
            MarketingService.GetChannels(Arg.Any<int>()).Returns(new Dictionary<string, string>
            {
                ["abc"] = "ABC"
            });
            var controller = CreateMarketingController();

            var result = controller.GetChannels() as JsonResult;
            var data = result.Data as IDictionary<string, string>;

            Assert.NotEmpty(data);
        }

        [Fact]
        public void ToggleMarketingLink_WhenCall_ToggleSuccessfully()
        {
            MarketingService.ToggleMarketingLink(Arg.Any<MarketingLinkParam>()).Returns(new ResultInfo { IsOk = true });
            var controller = CreateMarketingController();

            var result = controller.ToggleMarketingLink(new MarketingLinkViewModel()) as JsonResult;
            var data = result.Data as ResultInfo;

            Assert.True(data.IsOk);
        }

        [Fact]
        public void RenderUsingMarketLinkPage_WhenCall_ReturnCorrectView()
        {
            var controller = CreateMarketingController();
            MarketingService.GetUsingMarketingLinks(Arg.Compat.Any<int>()).Returns(new List<MarketingChannelLink> { new MarketingChannelLink { ChannelName = "ABC" } }.GroupBy(i => i.ChannelName));

            var result = controller.RenderUsingMarketLinkPage() as ViewResult;

            Assert.Contains("~/Views/UserPanel/AccountOverview/Partial/_ChannelLinkStatistic.cshtml", result.ViewName);
        }

        private MarketingController CreateMarketingController()
            => new MarketingController(SessionService, AppSetting, LanguageService, MarketingService, QrCodeService, FileSystem);
    }
}
