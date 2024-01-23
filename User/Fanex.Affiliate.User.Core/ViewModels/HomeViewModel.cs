namespace Fanex.Affiliate.User.Core.ViewModels
{
    public class HomeViewModel
    {
        public bool IsAuthenticated { get; set; }

        public string SignUpLink { get; set; }

        public string BannerLink { get; set; }

        public string PromotionYBLink { get; set; }

        public string Language { get; set; }

        public string ShortedLanguageCode { get; set; }
    }
}
