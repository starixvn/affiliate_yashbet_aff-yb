using System;
using System.Collections.Generic;

namespace Affiliate.YB.Jobs.Configurations
{
    public class TimeTrackerSetting
    {
        public IEnumerable<string> SourcePrefix { get; set; }

        public DateTime DefaultTime { get; set; }
    }
}
