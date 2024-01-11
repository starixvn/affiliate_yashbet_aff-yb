namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class OnUserIdParam : BaseParam
    {
        public int CustId { get; set; }

        public string NetSessionId { get; set; }

        public int ServerId { get; set; }

        public string Ip { get; set; }

        public int SubAccId { get; set; }

        public int On_UserId { get; set; }
    }
}