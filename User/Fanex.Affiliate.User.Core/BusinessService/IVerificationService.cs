using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IVerificationService
    {
        SendCodeResult SendCode(SendCodeParam param);

        bool IsValidOtp(VerificationOtpParam param);

        VerificationAction CreateActionLink(string key, string salt, int expirationInMinute);
    }
}
