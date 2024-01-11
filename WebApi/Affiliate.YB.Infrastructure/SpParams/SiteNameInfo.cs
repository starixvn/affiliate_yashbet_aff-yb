using Affiliate.YB.Core;

namespace Affiliate.YB.DTO
{
    public class SiteNameInfo : SettingInfo
    {
        public SiteNameInfo(IPrefixProvider prefixProvider) : base(prefixProvider)
        {
        }

        public override string SName => "BrandName";
    }
}