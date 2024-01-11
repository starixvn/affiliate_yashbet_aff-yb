
namespace Fanex.Affiliate.Admin.Core.DTO.Email
{
    public class EmailBase
    {

        public string RootUrl { get; set; }


        public string UserSiteUrl { get; set; }

        public string MailContact { get; set; }

        public string Language { get; set; }

        public string MailTo { get; set; }

        public bool IsHtml { get; set; }

        public string SiteFriendlyName { get; set; }
    }
}