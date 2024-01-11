using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    [CustomAuthorize]
    public class PlayerTransferBalanceHistoryController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IPlayerTransferBalanceService _playerTransferService;
        private readonly ICurrencyRepository _currencyRepo;
        private readonly IAccountRepository _accRepository;

        public PlayerTransferBalanceHistoryController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            ICurrencyRepository currencyRepository,
            IPlayerTransferBalanceService playerTransferBalanceService,
            IAccountRepository accountRepository)
            : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _playerTransferService = playerTransferBalanceService;
            _currencyRepo = currencyRepository;
            _accRepository = accountRepository;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            if (SessionService.UserSession.DisabledTransfer)
            {
                return Redirect(Url.Content("~/Notify/PageNotFound"));
            }

            return View("~/Views/UserPanel/PlayerTransferBalanceHistory/Index.cshtml");
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult GetHistory(PlayerTransferBalanceHistoryParam param)
        {
            if (SessionService.UserSession.DisabledTransfer)
            {
                return RedirectToRoute("Default", new
                {
                    controller = "Home",
                    action = "Index",
                });
            }

            var affInfo = _accRepository.GetAccountInfo(SessionService.UserSession.AffId);
            var baseCurrencyId = affInfo.PlayerCurrencyId;

            var currencies = _currencyRepo.GetBaseCurrencyList();
            var currencyName = currencies.FirstOrDefault(c => c.ExchangeId == baseCurrencyId)?.Code;
            var affId = SessionService.UserSession.AffId;
            ViewBag.BaseCurrency = currencyName;

            param.AffId = affId;
            param.BaseCurrency = baseCurrencyId;
            var model = _playerTransferService.GetPlayerTransferBalanceHistory(param);
            TranslateStatus(model);

            ViewBag.StartRowNumber = (param.PageIndex - 1) * param.PageSize + 1;

            return PartialView("~/Views/UserPanel/PlayerTransferBalanceHistory/_Content.cshtml", model);
        }

        private static void TranslateStatus(DTO.UserPanel.PlayerTransferBalance.PlayerTransferBalanceHistoryResult model)
        {
            foreach (var item in model.Items)
            {
                switch (item.Status)
                {
                    case TransferBalanceStatus.Completed:
                        item.Status = Lang.Completed;
                        break;
                    case TransferBalanceStatus.Pending:
                        item.Status = Lang.PendingTransaction;
                        break;
                    case TransferBalanceStatus.Failed:
                        item.Status = Lang.Failed;
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
