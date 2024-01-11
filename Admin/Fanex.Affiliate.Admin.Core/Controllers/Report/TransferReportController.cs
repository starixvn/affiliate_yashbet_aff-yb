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
    [FunctionAuthorize(FunctionName = Function.TransferPlayerReport)]
    public class TransferReportController : ReadOnlySessionBaseController
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITransferReportService _transferReportService;

        public TransferReportController(
            ISessionService sessionService,
            ICurrencyRepository currencyRepository,
            ITransferReportService transferReportService) : base(sessionService)
        {
            _currencyRepository = currencyRepository;
            _transferReportService = transferReportService;
        }


        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new ReportCommonViewModel
            {
                BaseCurrencyList = _currencyRepository.GetBaseCurrencies()
            };

            return View("~/Areas/Portal/Views/Report/TransferReport.cshtml", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetTransferReportData(TransferReportParam param)
        {
            return GenerateJsonNetResult(ModelState.IsValid ? _transferReportService.GetTransferReportData(param) : new EntityResult<TransferReport> { MainData = new List<TransferReport>() });
        }
    }
}
