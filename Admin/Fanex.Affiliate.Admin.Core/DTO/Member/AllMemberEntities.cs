using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.DTO.Member
{
    public class AllMemberEntities
    {
        public IEnumerable<ActiveMemberInfo> ActivedMemeberList { get; set; }

        public IEnumerable<InactiveMemberInfo> InactiveMemberList { get; set; }

        public string MemberNotExist { get; set; }
    }
}
