
namespace Fanex.Affiliate.Admin.Core.DTO.Affiliate
{
    public class AffiliateBasicInfo
    {
        public int AffId { get; set; }

        public string AffCode { get; set; }

        public string ParentAffiliateName { get; set; }

        public int GroupId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public int AffType { get; set; }

        public string Email { get; set; }

        public bool MailOption { get; set; }

        public string PreferredLanguage { get; set; }

        public string PlayerUsername { get; set; }

        public int PlayerCustId { get; set; }

        public bool IsUsingPrivateCommGroup { get; set; }
    }
}
