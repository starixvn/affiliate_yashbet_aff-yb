using Fanex.Data.Repository;

namespace Affiliate.YB.SpParams.Commands
{
    public class InsertAffiliateInfoCommand : NonQueryCommand
    {
        public int CustId { get; set; }

        public int SiteId { get; set; }

        public string AffiliateName { get; set; }

        public string ParentAffiliate { get; set; } = null;

        public string AffiliateCode { get; set; }

        public string UserName { get; set; }

        public override string GetSettingKey() => "Affiliate_AffiliateInfo_Set";

        public override bool IsValid() => true;
    }
}

