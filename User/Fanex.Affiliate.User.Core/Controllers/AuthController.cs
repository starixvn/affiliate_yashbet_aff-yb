using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.Extensions;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using System;
using System.Web.Mvc;
using System.Web.UI;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class AuthController : AuthBaseController
    {
        private const int CaptchaImgWidth = 56;
        private const int CaptchaImgHeight = 28;

        public AuthController(
            ISessionService sessionService,
            IAppSetting appSetting,
            ILanguageService languageService,
            IAuthService authService,
            ICaptchaService captchaService,
            IAccountRepository accountRepository) : base(sessionService, languageService, appSetting, captchaService, authService, accountRepository)
        {
        }

        [ChildActionOnly]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AuthenticationForm()
        {
            var viewModel = new AuthenticationFormViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated && SessionService.UserSession != null
            };

            if (viewModel.IsAuthenticated)
            {
                viewModel.UserName = SessionService.UserSession.UserName;
            }
            else
            {
                viewModel.PublicKey = AppSetting.RsaPublicKey;
            }

            return View("~/Views/Auth/AuthenticationForm.cshtml", viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignIn([Bind(Include = "LoginInfo,ValidationKey")] UserLoginParam userParam)
        {
            if (SessionService.VerificationSession != null)
            {
                return Json(new LoginResultInfo { IsOk = false, ShouldVerifyAccount = true });
            }

            var requestValidation = ValidateLoginRequest(userParam.ValidationKey, SessionService.CaptchaCode);
            if (!requestValidation.IsOk)
            {
                return Json(requestValidation);
            }

            userParam.LoginIp = Request.GetClientIP();
            userParam.Language = SessionService.CurrentLanguage;
            userParam.UserAgent = Request.UserAgent;
            userParam.IsVerifyingAccount = false;
            userParam.LoginInfo = Uri.UnescapeDataString(userParam.LoginInfo);
            var loginResult = AuthService.GetLoginResult(userParam, false);

            if (loginResult.Result.ShouldVerifyAccount)
            {
                SessionService.VerificationSession = new VerificationSession { AffId = loginResult.UserInfo.AffId, PreferredLanguage = loginResult.UserInfo.PreferredLanguage };
                return Json(loginResult.Result);
            }

            var userInfo = loginResult.UserInfo;
            if (userInfo != null)
            {
                if (!IsValidDomain(userInfo.AffType))
                {
                    return Json(GetErrorResult(Lang.NotAllowOnDomain));
                }

                if (loginResult.Result.IsOk)
                {
                    AssignLoginInfoAndSetAuth(userInfo, userParam.LoginIp);
                }
            }

            return Json(loginResult.Result);
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            DoSignOut();

            return Redirect("~/Home");
        }

        [HttpGet]
        public ActionResult AdminLogin(InternalLoginParam loginParam)
        {
            if (ModelState.IsValid)
            {
                var adminSessionId = AuthService.GetSessionId(loginParam.AccOnId);
                var hashedPassword = Generator.GenerateSHA512String(adminSessionId + loginParam.ChalengeStr);
                if (string.Equals(loginParam.AccKey, hashedPassword))
                {
                    var clientIpAddress = Request.GetClientIP();
                    var userLoginParam = new UserLoginParam
                    {
                        UserName = loginParam.CustName,
                        LoginIp = clientIpAddress,
                        Language = SessionService.CurrentLanguage,
                        UserAgent = Request.UserAgent,
                        IsVerifyingAccount = false
                    };

                    SessionService.IsInternal = true;
                    var loginResult = AuthService.GetLoginResult(userLoginParam, true);
                    var user = loginResult.UserInfo;

                    if (loginResult.Result.IsOk && user != null && IsValidDomain(user.AffType))
                    {
                        var adminId = loginParam.AccId * -1;
                        AssignLoginInfoAndSetAuth(loginResult.UserInfo, clientIpAddress, adminId);
                    }
                    else
                    {
                        return Redirect(Url.Content("~/Notify/PageNotFound"));
                    }
                }
                else
                {
                    return Redirect(Url.Content("~/Notify/PageNotFound"));
                }
            }

            return Redirect(Url.Content("~/UserPanel/Main/#AccountOverView"));
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None)]
        [CustomAuthorize]
        public ActionResult ReCheckin()
        {
            var onUserId = SessionService.UserSession.OnUserId;
            var isKickOut = onUserId > 0 && AuthService.Recheckin(SessionService.UserSession.OnUserId,
                SessionService.IsInternal);

            if (isKickOut)
            {
                DoSignOut();
            }

            return Json(new
            {
                IsKickOut = isKickOut,
                Message = isKickOut ? Lang.Msgkickout : string.Empty
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult CaptchaImage(byte type = 0)
        {
            var captcha = CaptchaService.GenerateCaptcha();
            if (type == 0)
            {
                SessionService.CaptchaCode = captcha;
            }
            else
            {
                SessionService.SignUpCaptchaCode = captcha;
            }

            return File(CaptchaService.GenerateImageBuffer(captcha, CaptchaImgWidth, CaptchaImgHeight), "image/png");
        }

        private void DoSignOut()
        {
            Session.Clear();
            AuthService.SignOut();
        }
    }
}