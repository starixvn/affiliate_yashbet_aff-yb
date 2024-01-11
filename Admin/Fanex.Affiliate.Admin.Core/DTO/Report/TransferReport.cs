using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Report
{
    public class TransferReport
    {
        public int TransId { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        public string AffName { get; set; }

        public string AffPlayerName { get; set; }

        public string PlayerName { get; set; }

        public string PlayerCurrency { get; set; }

        public string AffPlayerCurrency { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public decimal AffPlayerAmount { get; set; }

        public decimal BasedAmount { get; set; }

        public string CreatedDateText => CreatedDate.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
