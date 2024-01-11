using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Payment
{
    [Serializable]
    public class PaymentRequest
    {
        public long TransId { get; set; }

        [JsonIgnore]
        public DateTime TransDate { get; set; }

        public string Status { get; set; }

        public string AffName { get; set; }

        public decimal Amount { get; set; }

        [JsonIgnore]
        public DateTime CompletedDate { get; set; }

        public string Creator { get; set; }

        public string TransDateText => TransDate.ToString("MM/dd/yyyy HH:mm:ss");

        public string CompletedDateText => CompletedDate.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
