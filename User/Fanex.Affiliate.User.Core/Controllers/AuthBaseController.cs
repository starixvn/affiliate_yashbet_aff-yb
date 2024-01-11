using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class AuthBaseController : BaseController
    {
        private readonly ICaptchaService _captchaService;
        private readonly IAppSetting _appSetting;
        private readonly IAuthService _authService;
        protected readonly IAccountRepository _accountRepository;

        public AuthBaseController(
            ISessionService sessionService,
            ILanguageService languageService,
            IAppSetting appSetting,
            ICaptchaService captchaService,
            IAuthService authService,
            IAccountRepository accountRepository) : base(sessionService, languageService)
        {
            _captchaService = captchaService;
            _appSetting = appSetting;
            _authService = authService;
            _accountRepository = accountRepository;
        }

        protected ICaptchaService CaptchaService => _captchaService;

        protected IAppSetting AppSetting => _appSetting;

        protected IAuthService AuthService => _authService;

        protected ResultInfo ValidateLoginRequest(string inputCaptcha, string sessionCaptcha)
        {
            if (!Request.IsAjaxRequest())
            {
                return GetErrorResult(Lang.ErrAuthorized);
            }

            if (!ModelState.IsValid)
            {
                return GetErrorResult(string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }

            if (!_captchaService.IsValid(inputCaptcha, sessionCaptcha, Request.UserHostAddress))
            {
                return GetErrorResult(Lang.ValidationCode);
            }

            return new ResultInfo { IsOk = true };
        }

        protected bool IsValidDomain(string affType)
        {
            return _appSetting.AllowedAffiliateTypes.Any(t => t.Equals(affType));
        }

        protected void AssignLoginInfoAndSetAuth(AuthorizedUserInfo user, string clientIpAddress, int adminId = 0)
        {
            var onUserIdParam = GetOnUserIdParam(user.AffId, clientIpAddress, adminId);
            user.OnUserId = _authService.GetOnUserId(onUserIdParam);
            var accountInfo = _accountRepository.GetAccountInfo(user.AffId);

            SessionService.AssignSessions(user);
            SessionService.UserSession.BaseCurrency = accountInfo.DefaultBaseCurrency;
            AuthService.SignIn(user.UserName, false);
        }

        protected ResultInfo GetErrorResult(string message)
        {
            return new ResultInfo
            {
                IsOk = false,
                Message = message
            };
        }

        private OnUserIdParam GetOnUserIdParam(int affId, string clientIpAddress, int adminId)
        {
            return new OnUserIdParam
            {
                CustId = affId,
                ServerId = _appSetting.ServerId,
                Ip = clientIpAddress,
                NetSessionId = HttpContext.Session.SessionID,
                SubAccId = adminId,
                IsInternal = SessionService.IsInternal
            };
        }
    }
}
