using Fanex.Data.Repository;

namespace Fanex.Affiliate.Link.Criterias
{
    public class GeneralAffiliateInfoCriteria : NonQueryCommand
    {
        public int? AffId { get; set; }

        public string AffCode { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Get_GeneralAffInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}