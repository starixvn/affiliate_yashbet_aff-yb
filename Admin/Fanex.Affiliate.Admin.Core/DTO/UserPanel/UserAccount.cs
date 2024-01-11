using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    [Serializable]
    public class UserAccount
    {
        public int AffId { get; set; }

        public string AffName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [JsonIgnore]
        public string ContactApps { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }


        public string SiteUrl { get; set; }


        public string MoreSiteUrls { get; set; }

        public string NoSiteDescription { get; set; }

        public string PreferredPayment { get; set; }

        public string PlayerUsername { get; set; }

        public string NetellerAccountId { get; set; }

        public string MoneybookersEmail { get; set; }

        public string BankName { get; set; }

        public string BankAddress { get; set; }

        public string BankCode { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountNumber { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public bool? MailOption { get; set; }

        public IEnumerable<ContactApp> ContactAppList { get; set; }

        public bool HasWebSite => !string.IsNullOrEmpty(SiteUrl);
    }
}
