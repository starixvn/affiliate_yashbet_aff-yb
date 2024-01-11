
namespace Fanex.Affiliate.User.Core.MetalModels.Email
{
    public class NoticeEmailParam : EmailBaseParam
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string SiteUrl { get; set; }

        public string MoreSiteUrls { get; set; }

        public string NoSiteDesc { get; set; }
    }
}
