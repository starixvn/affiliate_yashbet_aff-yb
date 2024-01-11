namespace Fanex.Affiliate.User.Core.DTO.VerificationAccount
{
    public class SendCodeResult
    {
        public int NewCounter { get; set; }

        public long NewSendCodeTime { get; set; }

        public bool IsValidEmail { get; set; }

        public bool CanSendCode { get; set; }
    }
}
