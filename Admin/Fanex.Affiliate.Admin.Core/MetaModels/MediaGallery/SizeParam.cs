namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class SizeParam
    {
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Language { get; set; }

        public string Currency { get; set; }

        public string ImageType { get; set; }

        public byte AffType { get; set; } = Constants.AffType.Asian;

        public string DeviceType { get; set; }

        public byte UserType { get; set; }
    }
}
