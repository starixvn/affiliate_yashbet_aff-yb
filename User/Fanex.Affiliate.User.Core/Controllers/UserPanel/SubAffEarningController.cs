using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    [CustomAuthorize]
    public class SubAffEarningController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IStatementHistoryRepository _statementHistoryRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public SubAffEarningController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IStatementHistoryRepository statementHistoryRepository,
            ICurrencyRepository currencyRepository) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _statementHistoryRepository = statementHistoryRepository;
            _currencyRepository = currencyRepository;
        }

        [AjaxOnly]
        public ActionResult Index(SubAffComParam param)
        {
            param.AffId = SessionService.UserSession.AffId;
            param.IsInternal = SessionService.IsInternal;
            param.MonthDate = param.MonthDate ?? DateTimeHelper.NowDateTime.AddDays(-1);

            var subAffComs = _statementHistoryRepository.GetSubAffCommission(param);
            var requestPage = param.RequestPage;
            var vms = subAffComs.Select(s => new SubAffComViewModel
            {
                Commission = s.Commission,
                SubAffCommission = s.SubAffCommission,
                SubAffUserName = s.UserName
            });

            if (!Constants.RequestPage.Links.TryGetValue(requestPage, out BreadcrumbViewModel breadcrumb))
            {
                breadcrumb = Constants.RequestPage.Links[Constants.RequestPage.StatementHistory];
            }

            breadcrumb.Url = $"{breadcrumb.Url}?baseCurrency={param.BaseCurrency}";

            ViewBag.MonthDate = param.MonthDate;
            ViewBag.Breadcrumb = breadcrumb;
            ViewBag.BaseCurrencies = _currencyRepository.GetBaseCurrencyList();
            ViewBag.SelectedBaseCurrency = param.BaseCurrency;
            var path = param.IsOnlyContent
                ? "UserPanel/StatementHistory/StatementHistoryPartial/_SubAffEarningContent"
                : "UserPanel/StatementHistory/SubAffEarning";
            return View(path, vms);
        }
    }
}