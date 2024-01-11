using Fanex.Data.Repository;
using System;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetAffCustomersCriteria : CriteriaBase
    {
        private const string SP_NAME = "Aff_CustomerSel";

        private string Prefix { get; }

        public GetAffCustomersCriteria(DateTime fromDate, string prefix = null)
        {
            FromDate = fromDate;
            Prefix = prefix;
        }

        public override string GetSettingKey() => string.IsNullOrWhiteSpace(Prefix) ? SP_NAME: $"{Prefix}_{SP_NAME}";

        public override bool IsValid() => true;

        public DateTime FromDate { get; }
    }
}