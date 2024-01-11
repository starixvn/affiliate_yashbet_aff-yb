using Affiliate.YB.Core;
using System;

namespace Affiliate.YB.SpParams.Commands
{
    public class InsertImportTrackerCommand : IdenticalSuffixNonQueryCommand
    {
        public InsertImportTrackerCommand(IPrefixProvider prefixProvider = null) : base(prefixProvider) { }
        protected override string SuffixSettingKey => "Task_API_TimeTracker_Import";

        public override bool IsValid() => true;

        public string DataSource { get; set; }

        public DateTime CalledTime { get; set; }

        public bool IsReset { get; set; }

        public string KeyValue { get; set; } = String.Empty;

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = String.Empty;
    }
}
