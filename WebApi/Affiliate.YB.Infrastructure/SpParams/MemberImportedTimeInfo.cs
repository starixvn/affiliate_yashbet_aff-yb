using Affiliate.YB.Core;

namespace Affiliate.YB.DTO
{
    public class MemberImportedTimeInfo : SettingInfo
    {
        public MemberImportedTimeInfo(IPrefixProvider prefixProvider) : base(prefixProvider)
        {
        }

        public override string SName => "MemberInfoLastImportedTime";
    }
}