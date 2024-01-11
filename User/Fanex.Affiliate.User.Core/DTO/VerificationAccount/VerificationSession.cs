using System;

namespace Fanex.Affiliate.User.Core.DTO.VerificationAccount
{
    [Serializable]
    public class VerificationSession
    {
        public int AffId { get; set; }

        public string PreferredLanguage { get; set; }

        public int Counter { get; set; }

        public string Captcha { get; set; }

        public long SentCodeTime { get; set; }

        public string Token { get; set; }
    }
}
