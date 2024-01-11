using Fanex.Affiliate.User.Core.Utilities;
using QRCoder;
using System;
using System.Drawing;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class QrCodeUrlGeneratorService : IQrCodeGeneratorService
    {
        private const int SizeDefault = 7;
        private const int IconSizePercentDefault = 30;

        private readonly IImageHandler _imgHandler;

        public int Size { get; set; } = SizeDefault;

        public QrCodeUrlGeneratorService(IImageHandler imgHandler)
        {
            _imgHandler = imgHandler ?? new PngImageHandler();
        }

        public string GenerateBase64QrCode(string content, Image icon = null, bool includePrefix = false)
        {
            Bitmap bitmap = null;
            if (icon != null)
            {
                bitmap = new Bitmap(icon);
            }
            var base64Img = GenerateBase64QrCode(content, bitmap, includePrefix);
            bitmap?.Dispose();
            return base64Img;
        }

        public string GenerateBase64QrCode(string content, string icon = null, bool includePrefix = false)
        {
            Bitmap bitmap = null;
            if (icon != null)
            {
                Byte[] bitmapData = Convert.FromBase64String(icon);
                using (System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData))
                {
                    bitmap = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
                }
            }
            var base64Img = GenerateBase64QrCode(content, bitmap, includePrefix);
            bitmap?.Dispose();
            return base64Img;
        }

        private string GenerateBase64QrCode(string content, Bitmap icon = null, bool includePrefix = false)
        {
            var urlPayload = new QRCoder.PayloadGenerator.Url(content);
            var qrCoderData = QRCodeGenerator.GenerateQrCode(urlPayload, QRCodeGenerator.ECCLevel.H);
            var qrCode = new Base64QRCode(qrCoderData);
            var base64Img = icon == null ? qrCode.GetGraphic(Size) : qrCode.GetGraphic(Size, Color.Black, Color.White, icon, IconSizePercentDefault);
            if (includePrefix)
            {
                base64Img = $"{_imgHandler.Base64Prefix}{base64Img}";
            }
            return base64Img;
        }

        public void SaveImage(string base64Img, string path)
        {
            _imgHandler.Write(base64Img, path);
        }

        public Image GetImage(string path)
            => _imgHandler.GetImage(path);
    }
}
