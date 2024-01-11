using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.ViewModels;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IAuthService
    {
        LoginResultViewModel GetLoginResult(UserLoginParam userParam, bool isInternal);

        int GetOnUserId(OnUserIdParam onUserIdParam);

        string GetSessionId(int onUserId);

        bool Recheckin(int onUserId, bool isInternal);

        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }
}