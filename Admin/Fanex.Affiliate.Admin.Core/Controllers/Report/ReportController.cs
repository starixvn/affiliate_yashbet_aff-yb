using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Report;
using Fanex.Affiliate.Admin.Core.ViewModels.Report;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Report
{
    [FunctionAuthorize(FunctionName = Function.AffReport)]
    public class ReportController : ReadOnlySessionBaseController
    {
        private readonly IProductRepository _productRepository;

        private readonly ILanguageService _languageService;

        private readonly IAffiliateReportService _affiliateReportService;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly ICommissionGroupRepository _commissionGroupRepository;

        public ReportController(
            ISessionService sessionService,
            IProductRepository productRepository,
            ILanguageService languageService,
            IAffiliateReportService affiliateReportService,
            ICurrencyRepository currencyRepository,
            ICommissionGroupRepository commissionGroupRepository) : base(sessionService)
        {
            _productRepository = productRepository;
            _languageService = languageService;
            _affiliateReportService = affiliateReportService;
            _currencyRepository = currencyRepository;
            _commissionGroupRepository = commissionGroupRepository;
        }


        [HttpGet]
        [AllowByPassFunctionAuthorize]
        public ActionResult Index()
        {
            var viewModel = new AffiliateReportViewModel
            {
                ReportCutOffDate = _affiliateReportService.GetReportCutOffDate(),
                ProductList = _productRepository.GetProductList(),
                LanguageList = _languageService.GetLanguageList(),
                CurrencyList = _currencyRepository.GetCurrencies(),
                BaseCurrencyList = _currencyRepository.GetBaseCurrencies(),
                PaymentCurrencyList = _currencyRepository.GetPaymentCurrencies(),
                CommGroupList = _commissionGroupRepository.GetNormalCommGroupList(new GetGroupListParam { AffId = -1 }),
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetMainData(AffiliateReportParam param)
        {
            return GenerateJsonNetResult(_affiliateReportService.GetMainReportData(param));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetDetailData(AffiliateReportParam param)
        {
            return GenerateJsonNetResult(_affiliateReportService.GetDetailReportData(param));
        }
    }
}
