namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ICaptchaService
    {
        string GenerateCaptcha();

        bool IsValid(string inputCaptcha, string sessionCaptcha, string userHostAddress);

        byte[] GenerateImageBuffer(string captcha, int width, int height, bool isComplex = false);
    }
}
