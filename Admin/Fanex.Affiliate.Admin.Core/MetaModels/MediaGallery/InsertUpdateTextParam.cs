using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery
{
    public class InsertUpdateTextParam : UserInfoParam
    {
        public int TextId { get; set; }

        [Required]
        public string TextContent { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Currency { get; set; }

        public byte DeviceType { get; set; }

        public byte UserType { get; set; } = Constants.UserType.Player;
    }
}
