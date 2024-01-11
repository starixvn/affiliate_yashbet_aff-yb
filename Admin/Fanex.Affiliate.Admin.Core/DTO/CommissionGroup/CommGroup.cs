using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.CommissionGroup
{
    public class CommGroup
    {
        public int RowIdx { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        public string Creator { get; set; }

        public string Description { get; set; }

        public int NoAffs { get; set; }

        public decimal SubAffCommRate { get; set; }

        public bool IsActivePlayerSum { get; set; }

        public string CreatedDateText => CreatedDate.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
