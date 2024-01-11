namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class LinkGeneratingParam : LinkBaseParam
    {
        public int DomainId { get; set; }

        public string SelectedLanguage { get; set; }

        public string PreferredLanguage { get; set; }
    }
}
