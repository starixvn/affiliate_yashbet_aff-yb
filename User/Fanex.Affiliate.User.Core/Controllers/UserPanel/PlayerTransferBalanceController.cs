using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers.UserPanel
{
    [CustomAuthorize]
    public class PlayerTransferBalanceController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IPlayerTransferBalanceService _playerTransferBalanceService;

        public PlayerTransferBalanceController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService,
            IPlayerTransferBalanceService playerTransferBalanceService) : base(sessionService, languageService)
        {
            _appSetting = appSetting;
            _playerTransferBalanceService = playerTransferBalanceService;
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            if (SessionService.UserSession.DisabledTransfer)
            {
                return RedirectToRoute("Default", new
                {
                    controller = "Home",
                    action = "Index",
                });
            }

            return View("~/Views/UserPanel/PlayerTransferBalance/Index.cshtml");
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> SearchPlayer(string playerName)
        {
            if (SessionService.UserSession.DisabledTransfer)
            {
                return Json(new ResultInfo
                {
                    IsOk = false,
                    Message = Lang.ErrAuthorized
                });
            }
            var affId = SessionService.UserSession.AffId;
            var result = await _playerTransferBalanceService.GetPlayerTransferBalanceInfoAsync(affId, playerName);
            return Json(result);
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> SubmitTransferMoney(TransMoneyRequestViewModel param)
        {
            if (SessionService.UserSession.DisabledTransfer)
            {
                return Json(new ResultInfo
                {
                    IsOk = false,
                    Message = Lang.ErrAuthorized
                });
            }

            if (param.SearchName != param.SubmitName)
            {
                var result = new ResultInfo
                {
                    IsOk = false,
                    Message = Lang.DiffBetweenSearchAndUpdateName
                };
                return Json(result);
            }

            var submitResult = await _playerTransferBalanceService.SubmitTransferBalanceRequestAsync(param);

            return Json(submitResult);
        }
    }
}
