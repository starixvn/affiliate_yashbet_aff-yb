using Fanex.Affiliate.User.Core.MetalModels.Email;

namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class SendCodeParam
    {
        public int AffId { get; set; }

        public string Email { get; set; }

        public long SentCodeTime { get; set; }

        public string CurrentLanguage { get; set; }

        public int IntervalInMinute { get; set; }

        public VerificationAccountEmailParam VerificationEmailParam { get; set; }
    }
}
