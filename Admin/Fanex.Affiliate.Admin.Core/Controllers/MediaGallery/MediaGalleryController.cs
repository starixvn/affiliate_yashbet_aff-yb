using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.ViewModels.MediaGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.MediaGallery
{
    [FunctionAuthorize(FunctionName = Function.Media)]
    public class MediaGalleryController : ReadOnlySessionBaseController
    {
        private const int BannerListCategory = 1;
        private const string CasinoCategory = "Casino";
        private const string SportsbookCategory = "Sportsbook";
        private readonly ILanguageService _languageService;
        private readonly ICommonMediaGalleryService _commonMediaGalleryService;
        private readonly IBannerListService _bannerListService;
        private readonly IAffTypeService _affTypeService;
        private readonly IAppSetting _appSetting;

        public MediaGalleryController(
            ISessionService sessionService,
            ILanguageService languageService,
            ICommonMediaGalleryService commonMediaGalleryService,
            IBannerListService bannerListService,
            IAffTypeService affTypeService,
            IAppSetting appSetting) : base(sessionService)
        {
            _languageService = languageService;
            _commonMediaGalleryService = commonMediaGalleryService;
            _bannerListService = bannerListService;
            _affTypeService = affTypeService;
            _appSetting = appSetting;
        }

        public ActionResult Index(BannerParamFromHome param)
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.BannerList, Url = Url.Content("~/Portal/MediaGallery"), IsActive = true },
                new SubMenu { Name = Lang.LandingPageList, Url = Url.Content("~/Portal/LandingPageList") }
            };

            var sizeParam = new SizeParam
            {
                UserType = param.UserType
            };
            var bannerListCategory = _commonMediaGalleryService.GetCategoryList(BannerListCategory, param.UserType);

            var model = new BannerListViewModel
            {
                CategoryList = bannerListCategory.Where(c => string.IsNullOrEmpty(c.SubCategoryName)),
                SubCategoryList = bannerListCategory.Where(c => !string.IsNullOrEmpty(c.SubCategoryName)),
                SubCategoryCasinoList = bannerListCategory.Where(c => string.Equals(c.CategoryName, CasinoCategory, StringComparison.CurrentCultureIgnoreCase)
                                        && !string.IsNullOrEmpty(c.SubCategoryName)),
                SubCategorySportsbookList = bannerListCategory.Where(c => string.Equals(c.CategoryName, SportsbookCategory, StringComparison.CurrentCultureIgnoreCase)
                                        && !string.IsNullOrEmpty(c.SubCategoryName)),
                LanguageList = _languageService.GetLanguageList(),
                CurrencyList = _commonMediaGalleryService.GetCurrencyList(),
                SizeList = _bannerListService.GetSizeList(sizeParam),
                AffTypeList = _affTypeService.GetAffTypeList(),
                BannerRootUrl = _appSetting.BannerRootUrl,
                SubMenuList = subMenu,
                Param = param
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetCategoryList(byte userType)
        {
            return GenerateJsonNetResult(_commonMediaGalleryService.GetCategoryList(BannerListCategory, userType).Where(c => string.IsNullOrEmpty(c.SubCategoryName)));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetBannerRootUrl()
        {
            return GenerateJsonNetResult(_appSetting.BannerRootUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetSizeList(SizeParam param)
        {
            return GenerateJsonNetResult(_bannerListService.GetSizeList(param));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetBannerList(BannerParam param)
        {
            return GenerateJsonNetResult(_bannerListService.GetBannerList(param));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetBannerDetail(int selectedBannerId, byte userType)
        {
            return GenerateJsonNetResult(_bannerListService.GetBannerDetail(selectedBannerId, userType));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult InsertBanner(HttpPostedFileBase[] files, InsertUpdateMultipleBannersParam param)
        {
            if (ModelState.IsValid && files != null)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_bannerListService.InsertBanner(files, encodeParam));
            }

            return GenerateErrResult(Lang.InvalidBannerBrowser);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult UpdateBanner(HttpPostedFileBase files, InsertUpdateMultipleBannersParam param)
        {
            var isValidBannerId = param.BannerId >= Validation.MinId;
            if (ModelState.IsValid && isValidBannerId && (!string.IsNullOrEmpty(param.InputFileNames) || files != null))
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_bannerListService.UpdateBanner(files, encodeParam));
            }

            return GenerateErrResult(isValidBannerId ? Lang.PleaseEnterFileName : "Banner Id is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult DeleteBanners(DeleteBannerParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_bannerListService.DeleteBanners(param));
            }

            return GenerateErrResult("Please select Banner(s) to delete.");
        }

        private InsertUpdateMultipleBannersParam EncodeParam(InsertUpdateMultipleBannersParam param)
        {
            return new InsertUpdateMultipleBannersParam
            {
                BannerId = param.BannerId,
                InputFileNames = param.InputFileNames,
                Categories = Server.HtmlEncode(param.Categories),
                SubCategories = Server.HtmlEncode(param.SubCategories),
                Language = Server.HtmlEncode(param.Language),
                Currencies = Server.HtmlEncode(param.Currencies),
                AffTypes = param.AffTypes,
                DeviceTypes = param.DeviceTypes,
                UserType = param.UserType
            };
        }
    }
}