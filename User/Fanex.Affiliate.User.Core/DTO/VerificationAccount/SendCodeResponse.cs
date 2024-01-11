namespace Fanex.Affiliate.User.Core.DTO.VerificationAccount
{
    public class SendCodeResponse
    {
        public string VerificationToken { get; set; }

        public VerificationAction Action { get; set; }

        public bool IsValidEmail { get; set; }

        public bool CanSendCode { get; set; }
    }
}
