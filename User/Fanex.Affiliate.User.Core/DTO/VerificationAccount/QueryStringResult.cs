namespace Fanex.Affiliate.User.Core.DTO.VerificationAccount
{
    public class QueryStringResult
    {
        public bool IsValid { get; set; }

        public bool IsExpired { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }
    }
}
