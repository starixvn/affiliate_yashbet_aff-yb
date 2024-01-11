using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class UserAccountParam : AffiliateBasicInfoParam
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string ContactApps { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SiteUrl { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MoreSiteUrls { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NoSiteDescription { get; set; }

        [Required]
        public string PreferredPayment { get; set; }

        [SpParam(Ignored = true)]
        public string PlayerUsername { get; set; }

        public string NetellerAccountId { get; set; }

        public string MoneybookersEmail { get; set; }

        public string BankName { get; set; }

        public string BankAddress { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BankCode { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountNumber { get; set; }

        public bool MailOption { get; set; }

        public IEnumerable<ContactApp> ContactAppList { get; set; }

        public bool HasWebSite { get; set; }

        public int? Player_CustId { get; set; }

        public int? Player_Currency { get; set; }
    }
}
