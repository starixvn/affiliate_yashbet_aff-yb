using Fanex.Data.Repository;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetServerInfoCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "YB_Get_ServerInfo";

        public override bool IsValid() => true;
    }
}
