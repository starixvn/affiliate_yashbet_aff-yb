using Fanex.Affiliate.User.Core.Resources;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Constants
{
    public static class Menu
    {
        public static IDictionary<string, string> RevenueSharingMenuList => new Dictionary<string, string>
        {
            { "WhatWeHave", Lang.WhatWeHave },
            { "WhatWeOffer", Lang.WhatWeOffer },
            { "OurPlan", Lang.OurPlan },
            { "RevenueSharePayoutExample", Lang.RevenueSharePayoutExample },
            { "HowWePay", Lang.HowWePay }
        };

        public static IDictionary<string, string> TermAndConditionMenuList => new Dictionary<string, string>
        {
            { "ResponsibilitiesOfTheAffiliate", Lang.ResponsibilitiesOfTheAffiliate },
            { "AffiliationGuidelines", Lang.AffiliationGuidelines },
            { "AffiliateTagging", Lang.AffiliateTagging },
            { "RevenueSharingAndPayment", Lang.RevenueSharingAndPayment },
            { "PoliciesAndConfidentiality", Lang.PoliciesAndConfidentiality },
            { "LimitedLicense", Lang.LimitedLicense },
            { "LegalResponsibility", Lang.LegalResponsibility },
            { "Miscellaneous", Lang.Miscellaneous },
            { "TermAndTermination", Lang.TermAndTermination },
            { "RelationshipOfParties", Lang.RelationshipOfParties },
            { "Indemnity", Lang.Indemnity },
            { "Disclaimers", Lang.Disclaimers },
            { "LimitationOfLiability", Lang.LimitationOfLiability },
            { "IndependentInvestigation", Lang.IndependentInvestigation },
            { "GoverningLawAndArbitration", Lang.GoverningLawAndArbitration },
            { "InWitnessWhereOf", Lang.InWitnessWhereOf }
        };

        public static IDictionary<string, string> FaqMenuList => new Dictionary<string, string>
        {
            { "General", Lang.General },
            { "MarketingAndTracking", Lang.MarketingAndTracking },
            { "RevenueSharePayment", Lang.RevenueSharePayment }
        };
    }
}
