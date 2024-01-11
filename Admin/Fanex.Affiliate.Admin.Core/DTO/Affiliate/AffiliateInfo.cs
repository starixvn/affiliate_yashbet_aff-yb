using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Affiliate
{
    public class AffiliateInfo
    {
        public int AffId { get; set; }

        public int AffCode { get; set; }

        public int? ParentAffId { get; set; }

        public string ParentAffName { get; set; }

        public int? NoOfSubAff { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Language { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore]
        public DateTime? ApprovalDate { get; set; }

        public string CommissionGroup { get; set; }

        public decimal? BalanceBf { get; set; }

        public int? Member { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsClosed { get; set; }

        public string Currency { get; set; }

        public string PlayerCurrency { get; set; }

        public string PlayerUserName { get; set; }

        public string Phone { get; set; }

        public string RegistrationDateText => RegistrationDate.ToString("MM/dd/yyyy HH:mm:ss");

        public string ApprovalDateText => ApprovalDate?.ToString("MM/dd/yyyy HH:mm:ss") ?? "N/A";

        public bool IsUsingPrivateCommGroup { get; set; }

        [JsonIgnore]
        public string ApprovedBy { get; set; }

        public string ApprovedByText => string.IsNullOrEmpty(ApprovedBy) ? "N/A" : ApprovedBy;
    }
}
