
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Administrator
{
    public class AdministratorInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsOpen { get; set; }

        public string Creator { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateText => CreatedDate?.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
