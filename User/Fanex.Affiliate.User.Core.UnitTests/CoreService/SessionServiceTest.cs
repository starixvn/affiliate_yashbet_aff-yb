using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.CoreService
{
    public class SessionServiceTest
    {
        private const string UserSessionKey = "AffiliateB2C.UserSessionKey";

        private static HttpSessionState Session => HttpContext.Current.Session;

        [Fact]
        public void PropertyCatchaCode_WhenCall_PropertyStoreCorrectly()
        {
            SetUpHttpContextSession();
            var service = CreateService();
            var captchaCode = "12345";

            service.CaptchaCode = captchaCode;

            Assert.Equal(captchaCode, service.CaptchaCode);
        }

        [Fact]
        public void PropertyUserSession_WhenSessionNotSave_ReturnNull()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            var result = service.UserSession;

            Assert.Null(result);
        }

        [Fact]
        public void PropetyUseSession_WhenCall_ReturnCorrectSession()
        {
            var userSession = Constant.Session;
            SetUpHttpContextSession();
            Session[UserSessionKey] = userSession;

            var service = CreateService();

            Assert.Equal(userSession, service.UserSession);
        }

        [Fact]
        public void PropertyCurrentLanguage_WhenSessionNotExist_ReturnDefaultLang()
        {
            SetUpHttpContextSession();
            var service = CreateService();
            SessionStateUtility.RemoveHttpSessionStateFromContext(HttpContext.Current);

            var lang = service.CurrentLanguage;

            Assert.Equal(SystemLanguage.DefaultLanguageCode, lang);
        }

        [Fact]
        public void PropetyCurrentLanguage_WhenSessionLangIsEmpty_ReturnDefaultLang()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            service.CurrentLanguage = string.Empty;
            var lang = service.CurrentLanguage;

            Assert.Equal(SystemLanguage.DefaultLanguageCode, lang);
        }

        [Fact]
        public void PropertyCurrentLanguage_WhenCall_ReturnCorrectLang()
        {
            SetUpHttpContextSession();
            var service = CreateService();
            var lang = "vi-VN";

            service.CurrentLanguage = lang;

            Assert.Equal(lang, service.CurrentLanguage);
        }

        [Fact]
        public void PropertyIsInternal_WhenSessionNotExistKey_ReturnFalse()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            var result = service.IsInternal;

            Assert.False(result);
        }

        [Fact]
        public void PropetyIsInter_WhenCall_ReturnCorrectValue()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            service.IsInternal = true;

            Assert.True(service.IsInternal);
        }

        [Fact]
        public void SignUpCaptchaCode_WhenCall_ReturnCorrectValue()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            service.SignUpCaptchaCode = "12345";

            Assert.Equal("12345", service.SignUpCaptchaCode);
        }

        [Fact]
        public void PrivateKey_WhenCall_ReturnCorrectValue()
        {
            SetUpHttpContextSession();
            var service = CreateService();

            service.PrivateKey = "12345";

            Assert.Equal("12345", service.PrivateKey);
        }

        [Fact]
        public void AssignSession_WhenCallWithAutherizedUserHaveNoPreferLang_UseSessionHaveDefaultPreferLang()
        {
            SetUpHttpContextSession();
            var authUser = CreateAuthUser();
            authUser.PreferredLanguage = null;
            var service = CreateService();

            service.AssignSessions(authUser);

            Assert.Equal(authUser.AffId, service.UserSession.AffId);
            Assert.Equal(SystemLanguage.DefaultLanguageCode, service.UserSession.PreferredLanguage);
        }

        [Fact]
        public void AssignSession_WhenCallWithNotEmptyPreferLange_GetCorrectUserSession()
        {
            SetUpHttpContextSession();
            var authUser = CreateAuthUser();
            var service = CreateService();

            service.AssignSessions(authUser);

            Assert.Equal(authUser.AffId, service.UserSession.AffId);
            Assert.Equal(authUser.PreferredLanguage, service.UserSession.PreferredLanguage);
        }

        [Fact]
        public void PropertyVerificationSession_WhenCall_ReturnCorrectValue()
        {
            SetUpHttpContextSession();
            var verifySession = new VerificationSession
            {
                AffId = 1,
                Captcha = "1234",
                Counter = 2,
                SentCodeTime = 10000,
                Token = "abcd"
            };
            var service = CreateService();

            service.VerificationSession = verifySession;

            Assert.Equal(verifySession, service.VerificationSession);
        }

        private static SessionService CreateService()
            => new SessionService();

        private static AuthorizedUserInfo CreateAuthUser()
            => new AuthorizedUserInfo
            {
                AffId = 1,
                AffType = "asia",
                GroupName = "group",
                OnUserId = 1,
                Password = "abc",
                PreferredLanguage = "vi-VN",
                Status = "Pending",
                UserName = "name"
            };

        private static void SetUpHttpContextSession()
        {
            var request = new HttpRequest(null, "http://abco.com", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current, sessionContainer);
        }
    }
}
