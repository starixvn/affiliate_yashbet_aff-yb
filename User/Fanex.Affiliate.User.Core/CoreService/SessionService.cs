using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using System.Web;

namespace Fanex.Affiliate.User.Core.CoreService
{
    public class SessionService : ISessionService
    {
        private const string UserSessionKey = "AffiliateB2C.UserSessionKey";
        private const string CaptchaCodeKey = "AffiliateB2C.CaptchaCodeKey";
        private const string SingUpCaptchaCodeKey = "AffiliateB2C.SingUpCaptchaCodeKey";

        private const string CurrentLanguageKey = "AffiliateB2C.CurrentLanguageKey";
        private const string IsInternalKey = "AffiliateB2C.IsInternalKey";

        private const string RsaKeyPairKey = "AffiliateB2C.RsaKeyPairKey";
        private const string VerificationSessionKey = "AffiliateB2C.VerificationSessionKey";

        public string CaptchaCode
        {
            get { return HttpContext.Current.Session[CaptchaCodeKey] as string; }
            set { HttpContext.Current.Session[CaptchaCodeKey] = value; }
        }

        public UserSession UserSession
        {
            get
            {
                var temp = HttpContext.Current.Session[UserSessionKey];
                return temp != null ? (UserSession)temp : null;
            }
        }

        public string CurrentLanguage
        {
            get
            {
                var session = HttpContext.Current.Session;
                if (session == null)
                {
                    return SystemLanguage.DefaultLanguageCode;
                }

                var currentLanguage = session[CurrentLanguageKey] as string;

                return string.IsNullOrEmpty(currentLanguage) ? SystemLanguage.DefaultLanguageCode : currentLanguage;
            }
            set
            {
                HttpContext.Current.Session[CurrentLanguageKey] = value;
            }
        }

        public bool IsInternal
        {
            get { return (bool)(HttpContext.Current.Session[IsInternalKey] ?? false); }
            set { HttpContext.Current.Session[IsInternalKey] = value; }
        }

        public string SignUpCaptchaCode
        {
            get { return HttpContext.Current.Session[SingUpCaptchaCodeKey] as string; }
            set { HttpContext.Current.Session[SingUpCaptchaCodeKey] = value; }
        }

        public string PrivateKey
        {
            get { return HttpContext.Current.Session[RsaKeyPairKey] as string; }
            set { HttpContext.Current.Session[RsaKeyPairKey] = value; }
        }

        public void AssignSessions(AuthorizedUserInfo authorizedUser)
        {
            var userSession = new UserSession
            {
                AffId = authorizedUser.AffId,
                AffCode = authorizedUser.AffCode,
                UserName = authorizedUser.UserName,
                PreferredLanguage = string.IsNullOrEmpty(authorizedUser.PreferredLanguage) ?
                                        SystemLanguage.DefaultLanguageCode :
                                        authorizedUser.PreferredLanguage,
                OnUserId = authorizedUser.OnUserId,
                AffType = authorizedUser.AffType,
                GroupName = authorizedUser.GroupName,
                IsMaxSubLevel = authorizedUser.IsMaxSubLevel,
                IsDepositFeePercent = authorizedUser.IsDepositFeePercent,
                IsWithdrawalFeePercent = authorizedUser.IsWithdrawalFeePercent,
                DisabledTransfer = true //authorizedUser.DisabledTransfer //Hide Transfer Player
            };

            HttpContext.Current.Session[UserSessionKey] = userSession;
        }

        public VerificationSession VerificationSession
        {
            get { return HttpContext.Current.Session[VerificationSessionKey] as VerificationSession; }
            set { HttpContext.Current.Session[VerificationSessionKey] = value; }
        }
    }
}