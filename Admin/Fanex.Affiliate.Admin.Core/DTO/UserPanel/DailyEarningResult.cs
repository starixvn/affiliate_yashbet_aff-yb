using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    public class DailyEarningResult
    {
        public IEnumerable<DailyEarning> MainData { get; set; }

        public DailyEarning Total { get; set; }
    }
}
