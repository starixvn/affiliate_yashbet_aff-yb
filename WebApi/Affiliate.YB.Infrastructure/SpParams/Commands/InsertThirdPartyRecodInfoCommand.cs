using Affiliate.YB.Core;
using System;

namespace Affiliate.YB.SpParams.Commands
{
    public class InsertThirdPartyRecodInfoCommand : IdenticalSuffixNonQueryCommand
    {
        public InsertThirdPartyRecodInfoCommand(IPrefixProvider prefixProvider = null) : base(prefixProvider)
        {
        }

        protected override string SuffixSettingKey => "Task_API_Queue_Import";
        public override bool IsValid() => true;

        public string DataSource { get; set; }

        public string XmlData { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = String.Empty;
    }
}
