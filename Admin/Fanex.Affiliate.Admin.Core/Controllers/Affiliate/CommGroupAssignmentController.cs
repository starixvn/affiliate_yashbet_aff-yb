using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.CommGroup;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.Utilities.CommGroup;
using Fanex.Affiliate.Admin.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Affiliate
{
    [FunctionAuthorize(FunctionName = Function.RevenueGroup)]
    public class CommGroupAssignmentController : ReadOnlySessionBaseController
    {
        private const string PrivateGroupName = "[None]";

        private readonly ICommGroupCommonService _commGroupCommonService;

        private readonly ICommGroupAssignmentService _commGroupAssignmentService;

        private readonly IAffiliateService _affiliateService;

        private readonly IProductRepository _productRepository;

        private readonly ICommGroupValidation _commGroupValidation;

        public CommGroupAssignmentController(
            ISessionService sessionService,
            IProductRepository productRepository,
            IAffiliateService affiliateService,
            ICommGroupAssignmentService commGroupAssignmentService,
            ICommGroupCommonService commGroupCommonService,
            ICommGroupValidation commGroupValidation) : base(sessionService)
        {
            _affiliateService = affiliateService;
            _commGroupAssignmentService = commGroupAssignmentService;
            _productRepository = productRepository;
            _commGroupCommonService = commGroupCommonService;
            _commGroupValidation = commGroupValidation;
        }

        [HttpGet]
        public ActionResult AssignGroup(int affId)
        {
            var viewModel = new AssignNormalGroupViewModel();

            if (affId >= Validation.MinId)
            {
                var param = new GetGroupListParam { AffId = affId };
                var commGroupList = _commGroupAssignmentService.GetNormalCommGroupList(param);

                viewModel = new AssignNormalGroupViewModel
                {
                    CommGroupList = commGroupList,
                    SelectedGroup = param.GroupId,
                    AffInfo = _affiliateService.GetAffiliateBasicInfo(affId)
                };
            }

            return View("~/Areas/Portal/Views/Affiliate/AssignGroup.cshtml", viewModel);
        }

        [FunctionAuthorize(FunctionName = Function.RevenueGroup, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public async Task<ActionResult> AssignGroup(AssignGroupParam assignParam, AffInfoParam affInfo)
        {
            if (!ModelState.IsValid || assignParam.GroupId < Validation.MinId)
            {
                return GenerateModelStateErrResult();
            }

            assignParam.AffId = affInfo.AffId;
            AssignUserInfoParam(assignParam);

            var emailParam = new AffInformationEmail
            {
                RootUrl = SiteDomainHelper.GetRootUrl(this)
            };

            return GenerateJsonNetResult(await _commGroupAssignmentService.AssignGroup(assignParam, affInfo, emailParam));
        }

        [HttpGet]
        public ActionResult AssignPrivateGroup(int affId)
        {
            var viewModel = new AssignPrivateGroupViewModel();
            if (affId >= Validation.MinId)
            {
                var productList = _productRepository.GetProductList();
                viewModel = new AssignPrivateGroupViewModel
                {
                    ProductList = productList,
                    P2pProductList = productList.Where(p => p.IsP2P),
                    DeductionList = _commGroupCommonService.GetDeductionList(),
                    AffInfo = _affiliateService.GetAffiliateBasicInfo(affId)
                };
            }

            return View("~/Areas/Portal/Views/Affiliate/AssignPrivateGroup.cshtml", viewModel);
        }

        [FunctionAuthorize(FunctionName = Function.RevenueGroup, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public async Task<ActionResult> AssignPrivateGroup(CommGroupSettingParam commGroup, AffInfoParam affInfo)
        {
            if (!ModelState.IsValid)
            {
                return GenerateModelStateErrResult();
            }

            var result = _commGroupValidation.ValidateCommGroup(commGroup);
            if (result.IsSuccessful)
            {
                var assignParam = new CreateUpdateCommissionGroupParam
                {
                    UserId = SessionService.UserId,
                    GroupId = commGroup.GroupId,
                    GroupName = affInfo.UserName,
                    Description = PrivateGroupName,
                    TierSettingList = commGroup.TierSettingList,
                    P2pDeductionList = commGroup.P2pDeductionList,
                    PlatformFeeList = commGroup.PlatformFeeList,
                    DepositFee = commGroup.DepositFee,
                    WithdrawalFee = commGroup.WithdrawalFee,
                    SubAffCommRate = commGroup.SubAffCommRate,
                    IsActivePlayerSum = commGroup.IsActivePlayerSum,
                    Ip = Request.UserHostAddress
                };

                var emailParam = new AffInformationEmail
                {
                    RootUrl = SiteDomainHelper.GetRootUrl(this)
                };

                return GenerateJsonNetResult(await _commGroupAssignmentService.AssignPrivateGroup(assignParam, affInfo, emailParam));
            }

            return GenerateErrResult(result.Message);
        }
    }
}