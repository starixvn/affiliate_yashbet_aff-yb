using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public interface IImageHandler
    {
        void Write(Image img, string path, bool isOverWrite = false);
        void Write(string base64Img, string path, bool isOverWrite = false);
        string Base64Prefix { get; }

        Image GetImage(string path);
    }

    public class PngImageHandler : IImageHandler
    {
        public void Write(Image img, string path, bool isOverWrite = false)
        {
            var bitmap = new Bitmap(img);
            if (File.Exists(path))
            {
                if (isOverWrite)
                {
                    bitmap.Save(path, ImageFormat.Png);
                }
            }
            else
            {
                bitmap.Save(path, ImageFormat.Png);
            }
        }

        public void Write(string base64Img, string path, bool isOverWrite = false)
        {
            Byte[] bitmapData = Convert.FromBase64String(base64Img);
            MemoryStream streamBitmap = new MemoryStream(bitmapData);
            var img = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
            Write(img, path, isOverWrite);
        }

        public string Base64Prefix => "data:image/png;base64,";

        public Image GetImage(string path)
            => Image.FromFile(path);
    }
}
