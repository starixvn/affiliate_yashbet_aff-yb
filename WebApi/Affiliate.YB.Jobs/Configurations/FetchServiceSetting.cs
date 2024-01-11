using System;
using System.Collections.Generic;

namespace Affiliate.YB.Jobs.Configurations
{
    public class FetchServiceSetting
    {
        public string PullInfoCron { get; set; }

        public int AdjustmentValue { get; set; }

        public int MaxTimeRange { get; set; }

        public IEnumerable<string> SupportedPrefix { get; set; }

        public string DataSource { get; set; }
    }
}
