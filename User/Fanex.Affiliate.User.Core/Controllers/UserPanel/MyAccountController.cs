namespace Fanex.Affiliate.User.Core.Controllers
{
    using Constants;
    using DTO;
    using DTO.UserPanel;
    using Fanex.Affiliate.User.Core.BusinessService;
    using Fanex.Affiliate.User.Core.CoreService;
    using Fanex.Affiliate.User.Core.Repository.UserPanel;
    using Fanex.Affiliate.User.Core.Utilities;
    using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
    using Filters;
    using MetalModels;
    using Newtonsoft.Json;
    using Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    [CustomAuthorize]
    public class MyAccountController : BaseController
    {
        private const string SimplifiedChineseCode = "zh-CN";
        private const string TraditionalChineseCode = "zh-TW";
        private readonly ISessionService _sessionService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public MyAccountController(ISessionService sessionService,
            ILanguageService languageService,
            IAccountRepository accountRepository,
            ICurrencyRepository currencyRepository)
            : base(sessionService, languageService)
        {
            _sessionService = sessionService;
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Index()
        {
            var accountInfo = _accountRepository.GetAccountInfo(_sessionService.UserSession.AffId);
            var newAccountInfo = new AccountInformation
            {
                AffId = accountInfo.AffId,
                UserName = accountInfo.UserName,
                AffCode = accountInfo.AffCode,
                Gender = accountInfo.Gender,
                Email = Server.HtmlDecode(accountInfo.Email),
                Phone = Server.HtmlDecode(accountInfo.Phone),
                ContactApps = Server.HtmlDecode(accountInfo.ContactApps),
                Address = Server.HtmlDecode(accountInfo.Address),
                City = Server.HtmlDecode(accountInfo.City),
                State = Server.HtmlDecode(accountInfo.State),
                Country = Server.HtmlDecode(accountInfo.Country),
                SiteUrl = Server.HtmlDecode(accountInfo.SiteUrl),
                MoreSiteUrls = Server.HtmlDecode(accountInfo.MoreSiteUrls),
                PreferredPayment = Server.HtmlDecode(accountInfo.PreferredPayment),
                PlayerUsername = Server.HtmlDecode(accountInfo.PlayerUsername),
                Postal = Server.HtmlDecode(accountInfo.Postal),
                PreferredLanguage = accountInfo.PreferredLanguage,
                NoSiteDesc = Server.HtmlDecode(accountInfo.NoSiteDesc),
                IsRenderState = _sessionService.CurrentLanguage == SimplifiedChineseCode || _sessionService.CurrentLanguage == TraditionalChineseCode,
                HasWebSite = accountInfo.HasWebSite,
                AffCurrency = accountInfo.AffCurrency,
                DefaultBaseCurrency = accountInfo.DefaultBaseCurrency
            };

            if (!string.IsNullOrEmpty(newAccountInfo.ContactApps))
            {
                newAccountInfo.ContactAppList = JsonConvert.DeserializeObject<IEnumerable<ContactApp>>(newAccountInfo.ContactApps).OrderBy(p => p.Order);
            }

            var preferredLanguage = newAccountInfo.PreferredLanguage;
            var languageName = LanguageService.GetLanguageList().FirstOrDefault(l => l.Code.Equals(preferredLanguage))?.EnglishName ?? preferredLanguage;
            newAccountInfo.LanguageName = SystemLanguage.GetLanguageText(languageName);

            var baseCurrencyList = _currencyRepository.GetBaseCurrencyList();

            var model = new MyAccountViewModel
            {
                AccountInformation = newAccountInfo,
                BaseCurrencyList = CurrencyHelper.GetCurrencyList(baseCurrencyList, newAccountInfo.DefaultBaseCurrency)
            };

            return View("~/Views/UserPanel/MyAccount/index.cshtml", model);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AccountInformation model)
        {
            if (_sessionService.IsInternal)
            {
                return Json(new ResultInfo { IsOk = false, Message = Lang.ErrAuthorized });
            }

            if (!IsValidAppContactList(model.ContactAppList))
            {
                return Json(new ResultInfo { IsOk = false, Message = Error.RequireIMAccount });
            }

            var userSession = _sessionService.UserSession;
            var userAccount = _accountRepository.GetAccountInfo(userSession.AffId);
            model.PlayerUsername = !string.IsNullOrEmpty(userAccount?.PlayerUsername) ?
                userAccount.PlayerUsername :
                Server.HtmlEncode(model.PlayerUsername);

            var accountUpdateParam = new UpdateAccountParam
            {
                AffId = userSession.AffId,
                Phone = Server.HtmlEncode(model.Phone),
                SiteUrl = Server.HtmlEncode(model.SiteUrl ?? string.Empty),
                MoreSiteUrls = Server.HtmlEncode(model.MoreSiteUrls ?? string.Empty),
                NoSiteDesc = Server.HtmlEncode(model.NoSiteDesc ?? string.Empty),
                Ip = Request.UserHostAddress,
                Language = _sessionService.CurrentLanguage,
                DefaultBaseCurrency = model.DefaultBaseCurrency,
                ContactApps = JsonConvert.SerializeObject(model.ContactAppList.Select(p => new ContactApp
                {
                    Name = Server.HtmlEncode(p.Name),
                    Value = Server.HtmlEncode(p.Value),
                    Order = p.Order
                })),
            };

            var updatingResult = _accountRepository.UpdateAccountInformation(accountUpdateParam);
            if (updatingResult.IsOk)
            {
                userSession.BaseCurrency = accountUpdateParam.DefaultBaseCurrency;
            }

            return Json(updatingResult);
        }

        private static bool IsValidAppContactList(IEnumerable<ContactApp> contactAppList)
        {
            return contactAppList != null && !contactAppList.All(c => string.IsNullOrEmpty(c.Value));
        }
    }
}