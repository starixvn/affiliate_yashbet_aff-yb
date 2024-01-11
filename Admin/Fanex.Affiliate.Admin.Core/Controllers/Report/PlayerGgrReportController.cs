using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Report;
using Fanex.Affiliate.Admin.Core.ViewModels.Report;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Report
{
    [FunctionAuthorize(FunctionName = Function.AffReport)]
    public class PlayerGgrReportController : ReadOnlySessionBaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPlayerGgrReportService _playerGgrReportService;

        public PlayerGgrReportController(
            ISessionService sessionService,
            IProductRepository productRepository,
            ICurrencyRepository currencyRepository,
            IPlayerGgrReportService playerGgrReportService) : base(sessionService)
        {
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
            _playerGgrReportService = playerGgrReportService;
        }


        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new ReportCommonViewModel
            {
                ProductList = _productRepository.GetProductList(),
                BaseCurrencyList = _currencyRepository.GetBaseCurrencies()
            };

            return View("~/Areas/Portal/Views/Report/PlayerGgrReport.cshtml", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetPlayerGgrReportData(PlayerGgrReportParam param)
        {
            return GenerateJsonNetResult(ModelState.IsValid ? _playerGgrReportService.GetPlayerGgrReportData(param) : new EntityResult<PlayerGgrReport> { MainData = new List<PlayerGgrReport>() });
        }
    }
}
