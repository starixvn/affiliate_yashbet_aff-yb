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

        public static IDictionary<string, PrarentMenu> ProductMenuList => new Dictionary<string, PrarentMenu>
        {
            { "SportbookProducts", SportbookProductsMenu },
            { "CasinoProducts", CasinoProductsMenu }
        };

        private static readonly PrarentMenu SportbookProductsMenu = new PrarentMenu
        {
            Title = Lang.SportbookProducts,
            SubMenuList = new Dictionary<string, string>
            {
                { "Sportsbook", Lang.Sportsbook },
                { "VirtualSports", Lang.VirtualSports },
                { "NumberGame", Lang.NumberGame },
                { "RngKeno", Lang.RngKeno }
            }
        };

        private static readonly PrarentMenu CasinoProductsMenu = new PrarentMenu
        {
            Title = Lang.CasinoProducts,
            SubMenuList = new Dictionary<string, string>
            {
                { "AsiaGaming", Lang.AsiaGaming },
                { "AdvantPlay", Lang.AdvantPlay },
                { "AeSexy", Lang.AeSexy },
                { "Allbet", Lang.Allbet },
                { "AstroTech", Lang.AstroTech },
                { "Cq9", Lang.Cq9 },
                { "DreamGaming", Lang.DreamGaming },
                { "Ebet", Lang.Ebet },
                { "GamePlayInteractive", Lang.GamePlayInteractive },
                { "Habanero", Lang.Habanero },
                { "ISoftBetPragmaticPlay", Lang.ISoftBetPragmaticPlay },
                { "Jili", Lang.Jili },
                { "Joker", Lang.Joker },
                { "MicroGaming", Lang.MicroGaming },
                { "N2Live", Lang.N2Live },
                { "Playtech", Lang.Playtech },
                { "RedTiger", Lang.RedTiger },
                { "SaGaming", Lang.SaGaming },
                { "SabaCasino", Lang.SabaCasino },
                { "SpadeGaming", Lang.SpadeGaming },
                { "WmCasino", Lang.WmCasino },
                { "YggDrasil", Lang.YggDrasil }
            }
        };
    }

    public class PrarentMenu
    {
        public string Title { get; set; }

        public IDictionary<string, string> SubMenuList { get; set; }
    }
}
