using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Member
{
    public class PlayerActionInfo
    {
        public string UserName { get; set; }

        public bool IsRemoved { get; set; }

        public bool IsInclude { get; set; }

        public DateTime? LastActionDate { get; set; }
    }
}
