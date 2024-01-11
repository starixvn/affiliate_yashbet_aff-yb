using System.Drawing;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IQrCodeGeneratorService
    {
        string GenerateBase64QrCode(string content, string icon = null, bool includePrefix = false);

        string GenerateBase64QrCode(string content, Image icon = null, bool includePrefix = false);

        void SaveImage(string base64Img, string path);

        int Size { get; set; }

        Image GetImage(string path);
    }
}
