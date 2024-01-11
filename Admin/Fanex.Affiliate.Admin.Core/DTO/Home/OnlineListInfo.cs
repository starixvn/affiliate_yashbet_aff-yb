using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Home
{
    public class OnlineListInfo
    {
        public string UserName { get; set; }

        public DateTime? LoggedInDate { get; set; }

        public DateTime? LastRequestDate { get; set; }

        public string Ip { get; set; }

        public string Role { get; set; }

        public string LoggedInDateText => LoggedInDate?.ToString("MM/dd/yyyy HH:mm:ss");

        public string LastRequestDateText => LastRequestDate?.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
