using Affiliate.YB.Core;
using Affiliate.YB.SpParams.Commands;
using Fanex.Data;
using System;

namespace Affiliate.YB.DTO
{
    public class InsertMemberInfoCommand : IdenticalSuffixNonQueryCommand
    {
        public InsertMemberInfoCommand(IPrefixProvider prefixProvider) : base(prefixProvider)
        { }

        [SpParam(Ignored = true)]
        public string SettingPrefix { get; set; }

        [SpParam(Ignored = true)]
        public string SiteName { get; set; }

        public string XmlData { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = String.Empty;

        protected override string SuffixSettingKey => "Task_Aff_MemberInfo_Import";

        public override bool IsValid() => true;
    }
}
