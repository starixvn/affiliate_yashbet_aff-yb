using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    public class DailyEarningDetailResult
    {
        public IEnumerable<DailyEarningDetail> MainData { get; set; }

        public DailyEarningDetail Total { get; set; }
    }
}
