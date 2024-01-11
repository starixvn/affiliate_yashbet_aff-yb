using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Member
{
    public class MemberInfo : PlayerActionInfo
    {
        public int MemberId { get; set; }

        public string IbcUserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string AffName { get; set; }
    }
}
