using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.DTO.Report
{
    public class PlayerGgrReport
    {
        public string AffName { get; set; }

        public string PlayerName { get; set; }

        [JsonIgnore]
        public DateTime? RegistrationDate { get; set; }

        [JsonIgnore]
        public DateTime? FdmDate { get; set; }

        public decimal TotalTurnover { get; set; }

        public decimal TotalGgr { get; set; }

        public IDictionary<string, PlayerGgrProductInfo> ProductReportList { get; set; }

        public string RegistrationDateText => RegistrationDate?.ToString("MM/dd/yyyy HH:mm:ss") ?? string.Empty;

        public string FdmDateText => FdmDate?.ToString("MM/dd/yyyy HH:mm:ss") ?? string.Empty;
    }
}
