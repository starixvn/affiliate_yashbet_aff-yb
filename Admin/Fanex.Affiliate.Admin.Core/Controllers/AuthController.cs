using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Login;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Common;
using ONEbook.UM.AspNetMVC.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [CheckUMFilter]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly ISessionService _sessionService;
        private readonly IAppSetting _appSetting;

        public AuthController(
            ISessionService sessionService,
            IAuthRepository authRepository,
            IAppSetting appSetting)
        {
            _sessionService = sessionService;
            _authRepository = authRepository;
            _appSetting = appSetting;
        }

        [HttpGet]
        public ActionResult RecheckIn()
        {
            return View("~/Areas/Portal/Views/Auth/RecheckIn.cshtml");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Ping()
        {
            var isKickedOut = true;
            var isAuthorized = HttpContext.Request.IsAuthenticated && HttpContext.User.Identity.IsAuthenticated;
            if (isAuthorized && _sessionService.OnUserId > 0 && _sessionService.UserId > 0)
            {
                isKickedOut = _authRepository.ReCheckin(_sessionService.OnUserId);
            }
            return Json(new { IsKickedOut = isKickedOut, Message = isKickedOut ? Lang.MsgKickout : string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult Login(LoginParam param)
        {
            param.UserHostName = HttpContext.Request.UserHostName;
            param.Ip = Request.GetClientIP();

            var result = _authRepository.Login(param);

            var permissionList = param.UserId > 0 ? _authRepository.GetPermissionList(param.UserId) : new List<UserPermission>();

            if (result.IsSuccessful && param.UserId > 0)
            {
                if (permissionList.Any())
                {
                    var authTicket = new FormsAuthenticationTicket(param.UserName, true, 20);
                    var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        HttpOnly = true,
                        Secure = FormsAuthentication.RequireSSL,
                        Path = FormsAuthentication.FormsCookiePath,
                        Domain = FormsAuthentication.CookieDomain,
                        Expires = authTicket.Expiration
                    };

                    _sessionService.UserId = param.UserId;
                    _sessionService.UserPermissionList = permissionList;
                    var onUserId = _authRepository.LoginOnUser(param, HttpContext.Session.SessionID, _appSetting.ServerId);
                    _sessionService.OnUserId = onUserId;

                    HttpContext.ApplicationInstance.Context.Session["UserName"] = param.UserName;

                    HttpContext.Response.SetCookie(cookie);

                    result.Message = "OK";
                }
                else
                {
                    result = new ResultInfo
                    {
                        Code = ErrCode.ErrNoPermissionOnAnyFunctions,
                        Message = "Your account is not configured to access any functions! Please contact your administrator"
                    };
                }
            }

            return new JsonNetResult
            {
                Data = result
            };
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var context = HttpContext;
            _authRepository.Logout(_sessionService.OnUserId);
            FormsAuthentication.SignOut();
            context.Session.Clear();
            context.Session.Abandon();
            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
