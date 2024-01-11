namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class PasswordRecoveryParam
    {
        public int AffId { get; set; }

        public string ValidCode { get; set; }

        public string NewPassword { get; set; }

        public int ErrCode { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }
    }
}
