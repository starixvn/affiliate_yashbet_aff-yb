using Fanex.Data.Repository;
using System;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetFixedCustomerCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "YB_Get_Customer_xtest";

        public override bool IsValid() => true;
    }
}
