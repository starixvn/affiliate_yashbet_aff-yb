using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.ViewModels.MediaGallery;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.MediaGallery
{
    [FunctionAuthorize(FunctionName = Function.Media)]
    public class TextListController : ReadOnlySessionBaseController
    {
        private const int TextListCategory = 2;
        private readonly ILanguageService _languageService;
        private readonly ICommonMediaGalleryService _commonMediaGalleryService;
        private readonly ITextListService _textListService;

        public TextListController(
            ISessionService sessionService,
            ILanguageService languageService,
            ICommonMediaGalleryService commonMediaGalleryService,
            ITextListService textListService) : base(sessionService)
        {
            _languageService = languageService;
            _commonMediaGalleryService = commonMediaGalleryService;
            _textListService = textListService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.BannerList, Url = Url.Content("~/Portal/MediaGallery") },
                new SubMenu { Name = Lang.TextList, Url = Url.Content("~/Portal/TextList"), IsActive = true },
                new SubMenu { Name = Lang.LandingPageList, Url = Url.Content("~/Portal/LandingPageList") }
            };

            var model = new TextListViewModel
            {
                CategoryList = _commonMediaGalleryService.GetCategoryList(TextListCategory, UserType.All).Where(c => string.IsNullOrEmpty(c.SubCategoryName)),
                LanguageList = _languageService.GetLanguageList(),
                CurrencyList = _commonMediaGalleryService.GetCurrencyList(),
                SubMenuList = subMenu
            };

            return View("~/Areas/Portal/Views/MediaGallery/TextList.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetCategoryList(byte userType)
        {
            return GenerateJsonNetResult(_commonMediaGalleryService.GetCategoryList(TextListCategory, userType).Where(c => string.IsNullOrEmpty(c.SubCategoryName)));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTextList(TextParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_textListService.GetTextList(param));
            }

            return GenerateJsonNetResult(new EntityResult<TextInfo>
            {
                MainData = new List<TextInfo>(),
                TotalItem = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTextDetail(int selectedTextId, byte userType)
        {
            return GenerateJsonNetResult(_textListService.GetTextDetail(selectedTextId, userType));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult InsertText(InsertUpdateTextParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_textListService.InsertText(encodeParam));
            }

            return GenerateErrResult("Please input text to upload.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult UpdateText(InsertUpdateTextParam param)
        {
            var isValidTextId = param.TextId >= Validation.MinId;
            if (ModelState.IsValid && isValidTextId)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_textListService.UpdateText(encodeParam));
            }

            return GenerateErrResult(isValidTextId ? "Please input text to update." : "TextId is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Media, IsHost = true)]
        public ActionResult DeleteTexts(DeleteTextParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_textListService.DeleteTexts(param));
            }

            return GenerateErrResult("Please select texts to delete.");
        }

        private InsertUpdateTextParam EncodeParam(InsertUpdateTextParam param)
        {
            return new InsertUpdateTextParam
            {
                TextId = param.TextId,
                TextContent = Server.HtmlEncode(param.TextContent),
                Category = param.Category,
                Language = param.Language,
                Currency = param.Currency,
                DeviceType = param.DeviceType,
                UserType = param.UserType
            };
        }
    }
}
