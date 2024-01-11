namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ITwoFactorAuthService
    {
        string GenerateHotp(string secretKey, int counter);

        bool VerifyHotp(string secretKey, int counter, string otp);
    }
}
