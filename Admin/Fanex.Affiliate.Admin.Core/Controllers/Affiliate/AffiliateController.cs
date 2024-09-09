using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Repository.Affiliate;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.ViewModels.Affiliate;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Affiliate
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class AffiliateController : ReadOnlySessionBaseController
    {
        private readonly IAffiliateInfoRepository _affiliateInfoRepository;

        private readonly ICommissionGroupRepository _commissionGroupRepository;

        private readonly ILanguageService _languageService;

        public AffiliateController(
            ISessionService sessionService,
            IAffiliateInfoRepository affiliateInfoRepository,
            ICommissionGroupRepository commissionGroupRepository,
            ILanguageService languageService) : base(sessionService)
        {
            _commissionGroupRepository = commissionGroupRepository;
            _affiliateInfoRepository = affiliateInfoRepository;
            _languageService = languageService;
        }

        [HttpGet]
        public ActionResult Index(ParamFromHome param, int groupId = 0)
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AffiliateInfo, Url = Url.Content("~/Portal/Affiliate"), FunctionName = Function.Affiliate },
                new SubMenu { Name = Lang.PaymentRequest, Url = Url.Content("~/Portal/PaymentRequest"), FunctionName = Function.Payment, IsActive = true },
                new SubMenu { Name = Lang.CommGroup, Url = Url.Content("~/Portal/RevenueGroup"), FunctionName = Function.RevenueGroup }
            };

            var groupListParam = new GetGroupListParam { AffId = -1 };
            var viewModel = new AffiliateInfoViewModel
            {
                CommGroupList = _commissionGroupRepository.GetNormalCommGroupList(groupListParam),
                LanguageList = _languageService.GetLanguageList(),
                SelectedGroupId = groupId,
                Param = param,
                SubMenuList = subMenu
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetAffiliateList(AffiliateInfoParam param)
        {
            if (int.TryParse(param.AffCode, out _) || string.IsNullOrEmpty(param.AffCode))
            {
                param.UserId = SessionService.UserId;
                var model = _affiliateInfoRepository.GetAffiliateList(param);

                return GenerateJsonNetResult(model);
            }

            return GenerateJsonNetResult(new EntityResult<AffiliateInfo>
            {
                MainData = new List<AffiliateInfo>(),
                TotalItem = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Affiliate, IsHost = true)]
        public ActionResult ToggleFavorite(FavoriteParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_affiliateInfoRepository.ToggleFavorite(param));
            }

            return GenerateErrResult("Update Favorite failed!");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.EditAffDetails, IsHost = true)]
        public ActionResult ChangePassword(PasswordParam param)
        {
            if (ModelState.IsValid)
            {
                param.Password = CryptographyHelper.Encrypt(param.Password);
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_affiliateInfoRepository.ChangePassword(param));
            }
            return GenerateErrResult("Change Password is failed!");
        }
    }
}
