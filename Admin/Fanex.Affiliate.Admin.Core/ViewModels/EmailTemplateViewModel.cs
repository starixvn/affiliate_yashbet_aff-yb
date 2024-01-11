using Fanex.Affiliate.Admin.Core.Configurations;

namespace Fanex.Affiliate.Admin.Core.ViewModels
{
    public class EmailTemplateViewModel
    {
        private static readonly IAppSetting _appSetting = AppSetting.Instance;


        public string RootUrl { get; set; }


        public string UserSiteUrl { get; set; }


        public string ImageRootUrl { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SiteFriendlyName { get; } = _appSetting.SiteFriendlyName;

        public string SiteName { get; } = _appSetting.SiteName;

    }
}
