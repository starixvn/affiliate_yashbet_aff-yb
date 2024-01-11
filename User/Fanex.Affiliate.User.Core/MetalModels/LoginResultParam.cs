namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class LoginResultParam : AuthResultBaseParam
    {
        public bool IsSuccessful { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
