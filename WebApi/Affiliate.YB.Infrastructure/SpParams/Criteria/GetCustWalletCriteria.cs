using Fanex.Data.Repository;
using System;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetCustWalletCriteria : CriteriaBase
    {
        public GetCustWalletCriteria(DateTime fromDate) => FromDate = fromDate;

        public override string GetSettingKey() => "YB_Get_CustWallet";

        public override bool IsValid() => true;

        public DateTime FromDate { get; set; }
    }
}
