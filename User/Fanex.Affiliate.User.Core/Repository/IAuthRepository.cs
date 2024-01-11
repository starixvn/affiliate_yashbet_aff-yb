using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.MetalModels;

namespace Fanex.Affiliate.User.Core.Repository
{
    public interface IAuthRepository
    {
        AuthorizedUserInfo GetUserInfo(string userName, bool isInternal);

        LoginResultInfo UpdateLoginResult(LoginResultParam loginResultParam);

        int GetOnUserId(OnUserIdParam onUserIdParam);

        string GetSessionId(int onUserId);

        bool Recheckin(int onUserId, bool isInternal);
    }
}