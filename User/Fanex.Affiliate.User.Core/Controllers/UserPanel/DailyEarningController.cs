namespace Fanex.Affiliate.User.Core.Controllers
{
    using Constants;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Fanex.Affiliate.User.Core.Utilities;
    using Filters;
    using MetalModels.UserPanel;
    using System.Web.Mvc;
    using ViewModels.UserPanel;

    [CustomAuthorize]
    public class DailyEarningController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatementHistoryRepository _statementHistoryRepository;
        private readonly ICurrencyService _currencyService;

        public DailyEarningController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatementHistoryRepository statementHistoryRepository,
            ICurrencyService currencyService
            ) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _statementHistoryRepository = statementHistoryRepository;
            _currencyService = currencyService;
        }

        [AjaxOnly]
        public ActionResult Index(DailyEarningParam param)
        {
            param.AffId = SessionService.UserSession.AffId;
            param.IsInternal = SessionService.IsInternal;
            param.ProductId = param.ProductId ?? Product.Sportsbook;
            param.MonthDate = param.MonthDate ?? DateTimeHelper.NowDateTime.AddDays(-1);

            var viewModel = new DailyEarningViewModel
            {
                ProductList = param.IsOnlyContent ? null : _statementHistoryRepository.GetProductList(param.IsInternal),
                Data = _statementHistoryRepository.GetDailyEarning(param),
                SelectedProduct = param.ProductId.Value
            };
            ViewBag.MonthDate = param.MonthDate;
            ViewBag.BaseCurrencies = _currencyService.GetBaseCurrencyList();
            ViewBag.SelectedBaseCurrency = param.BaseCurrency;
            var viewPath = param.IsOnlyContent ? "~/Views/UserPanel/StatementHistory/StatementHistoryPartial/_DailyEarningContent.cshtml" : "~/Views/UserPanel/StatementHistory/DailyEarning.cshtml";

            return View(viewPath, viewModel);

        }

    }
}