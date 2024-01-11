using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class DailyEarningResult
    {
        public IEnumerable<DailyEarning> MainDataList { get; set; }

        public DailyEarning Total { get; set; }
    }
}
