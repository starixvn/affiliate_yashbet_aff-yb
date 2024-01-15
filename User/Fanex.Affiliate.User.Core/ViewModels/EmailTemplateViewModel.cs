using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Affiliate.User.Core.ViewModels
{
    public class EmailTemplateViewModel
    {
        public string RootUrl { get; set; }

        public string ImageRootUrl { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "<Pending>")]
        public string Copyright => string.Format(SiteFriendlyNameHelper.ReplaceB2C(Lang.Copyright), DateTimeHelper.CurrentYear);
    }
}
