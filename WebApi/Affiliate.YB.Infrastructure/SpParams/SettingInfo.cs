using Affiliate.YB.Core;
using Affiliate.YB.SpParams.Criteria;

namespace Affiliate.YB.DTO
{
    public class SettingInfo : IdenticalSurfixCriteriaBase
    {
        public SettingInfo(IPrefixProvider prefixProvider) : base(prefixProvider)
        {
        }

        public virtual string SName { get; set; }

        protected override string SuffixSettingKey => "Task_Aff_GetSetting";

        public override bool IsValid() => true;
    }
}
