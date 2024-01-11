using Fanex.Data.Repository;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetTpGameTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "YB_Get_TpGameType";

        public override bool IsValid() => true;
    }
}
