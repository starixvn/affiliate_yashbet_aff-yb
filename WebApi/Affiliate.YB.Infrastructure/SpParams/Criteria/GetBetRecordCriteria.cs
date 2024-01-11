using Fanex.Data.Repository;
using System;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetBetRecordCriteria : CriteriaBase
    {
        public GetBetRecordCriteria(DateTime fromDate, DateTime toDate, int pageIdex, int pageSize)
        {
            FromDate = fromDate;
            ToDate = toDate;
            PageIndex = pageIdex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() => "YB_Get_TpBetRecord";

        public override bool IsValid() => true;

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
