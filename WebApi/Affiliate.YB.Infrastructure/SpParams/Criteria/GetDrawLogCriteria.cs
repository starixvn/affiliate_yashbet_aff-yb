using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.YB.SpParams.Criteria
{
    public class GetDrawLogCritearia : CriteriaBase
    {
        public GetDrawLogCritearia(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public int Id { get; set; }

        public int BatchCount { get; set; }

        public override string GetSettingKey()
            => "API_DrawLog_Get";

        public override bool IsValid()
            => true;
    }
}
