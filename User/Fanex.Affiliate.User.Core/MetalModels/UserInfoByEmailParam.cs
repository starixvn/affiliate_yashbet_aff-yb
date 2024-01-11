namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class UserInfoByEmailParam
    {
        public string Email { get; set; }

        public string ValidCode { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string Language { get; set; }
    }
}
