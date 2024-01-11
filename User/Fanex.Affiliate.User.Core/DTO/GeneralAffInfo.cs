namespace Fanex.Affiliate.User.Core.DTO
{
    public class GeneralAffInfo
    {
        public int AffId { get; set; }

        public string AffCode { get; set; }

        public bool IsMaxSubLevel { get; set; }

        public bool IsValidAff => AffId > 0 && !string.IsNullOrEmpty(AffCode) && !IsMaxSubLevel;
    }
}
