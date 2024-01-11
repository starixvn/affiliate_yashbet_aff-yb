using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;

namespace Fanex.Affiliate.User.Core.CoreService
{
    public interface ISessionService
    {
        UserSession UserSession { get; }

        string CaptchaCode { get; set; }

        string CurrentLanguage { get; set; }

        bool IsInternal { get; set; }

        string SignUpCaptchaCode { get; set; }

        string PrivateKey { get; set; }

        void AssignSessions(AuthorizedUserInfo authorizedUser);

        VerificationSession VerificationSession { get; set; }
    }
}