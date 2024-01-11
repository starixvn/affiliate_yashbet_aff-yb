using Fanex.Affiliate.User.Core.MetalModels.Attributes;
using Fanex.Affiliate.User.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class AccountInformation
    {
        public int AffId { get; set; }

        public string AffCode { get; set; }

        public string UserName { get; set; }

        public bool Gender { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postal { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "errContactNumberEmpty")]
        public string Phone { get; set; }

        [RequiredByChecked(DependentPropertyName = "HasWebSite", IsChecked = true, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "WebsiteURLIsRequired")]
        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string SiteUrl { get; set; }

        public string MoreSiteUrls { get; set; }

        [RequiredByChecked(DependentPropertyName = "HasWebSite", IsChecked = false, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "DescWebsite")]
        public string NoSiteDesc { get; set; }

        public string PreferredLanguage { get; set; }

        public string PreferredPayment { get; set; }

        public string PlayerUsername { get; set; }

        public string State { get; set; }

        public bool IsRenderState { get; set; }

        public bool IsPlayerAccount => !string.IsNullOrEmpty(PreferredPayment) && PreferredPayment.Equals("Player", StringComparison.OrdinalIgnoreCase);

        public IEnumerable<string> MoreWebsiteUrlList => string.IsNullOrEmpty(MoreSiteUrls) ? new string[0] : MoreSiteUrls.Split(';');

        public string LanguageName { get; set; }

        public bool HasWebSite { get; set; }

        public string ContactApps { get; set; }

        public IEnumerable<ContactApp> ContactAppList { get; set; }

        public string AffCurrency { get; set; }

        public int DefaultBaseCurrency { get; set; }

        public decimal TransferLimit { get; set; }

        public int PlayerCurrencyId { get; set; }
    }
}