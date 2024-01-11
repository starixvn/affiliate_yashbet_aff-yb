namespace Fanex.Affiliate.User.Core.MetalModels.Email
{
    public class VerificationAccountEmailParam : EmailBaseParam
    {
        public string Code { get; set; }

        public string Ip { get; set; }
    }
}
