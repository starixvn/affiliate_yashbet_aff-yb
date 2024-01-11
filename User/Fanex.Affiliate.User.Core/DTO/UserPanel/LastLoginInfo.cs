using System;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class LastLoginInfo
    {
        public string AffCode { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string LastLoginIP { get; set; }
    }
}
