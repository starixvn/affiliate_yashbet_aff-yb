
namespace Fanex.Affiliate.Admin.Core.DTO.CommissionGroup
{
    public class TierSetting
    {
        public int GroupId { get; set; }

        public int TierId { get; set; }

        public int GroupProductId { get; set; }

        public decimal MinAmount { get; set; }

        public int MinAp { get; set; }

        public decimal Commission { get; set; }
    }
}
