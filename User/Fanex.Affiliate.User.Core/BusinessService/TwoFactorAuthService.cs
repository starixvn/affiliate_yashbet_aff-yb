using OtpNet;
using System.Text;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        public string GenerateHotp(string secretKey, int counter)
        {
            var base32String = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secretKey));
            var base32Bytes = Base32Encoding.ToBytes(base32String);

            Hotp hotp = new Hotp(base32Bytes);

            return hotp.ComputeHOTP(counter);
        }

        public bool VerifyHotp(string secretKey, int counter, string otp)
        {
            var base32String = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secretKey));
            var base32Bytes = Base32Encoding.ToBytes(base32String);
            Hotp hotp = new Hotp(base32Bytes);

            return hotp.VerifyHotp(otp, counter);
        }
    }
}
