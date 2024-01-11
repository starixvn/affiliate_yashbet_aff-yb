namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class CommissionSetting
    {
        public int TierId { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public int MinQm { get; set; }

        public int MaxQm { get; set; }

        public decimal Commission { get; set; }
    }
}
