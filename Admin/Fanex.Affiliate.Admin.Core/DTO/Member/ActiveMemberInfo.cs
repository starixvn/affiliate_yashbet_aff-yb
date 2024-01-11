using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Member
{
    public class ActiveMemberInfo : InactiveMemberInfo
    {
        public int IbcCustId { get; set; }

        public string IbcUserName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateText => CreatedDate?.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
