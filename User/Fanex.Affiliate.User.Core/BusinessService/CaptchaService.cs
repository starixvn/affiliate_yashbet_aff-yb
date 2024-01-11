using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IAppSetting _appSetting;

        private const int RANDOM_BEGIN_NUMBER = 1000;
        private const int RANDOM_END_NUMBER = 9999;

        private readonly string[] fontNameList = new[] {
         "Comic Sans MS",
         "Arial",
         "Times New Roman",
         "Georgia",
         "Verdana",
         "Geneva"
        };

        private readonly FontStyle[] fontStyleList = new[] {
         FontStyle.Bold,
         FontStyle.Italic,
         FontStyle.Regular,
         FontStyle.Strikeout,
         FontStyle.Underline
        };

        private readonly HatchStyle[] hatchStyleList = new[] {
         HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
         HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical, HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
         HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
         HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
         HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal, HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
         HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal, HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
         HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard, HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
         HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis, HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
         HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
        };

        private readonly int[] fontEmSizeList = new[] { 16, 18, 20 };

        public CaptchaService(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public string GenerateCaptcha()
            => new Random().Next(RANDOM_BEGIN_NUMBER, RANDOM_END_NUMBER).ToString();

        public bool IsValid(string inputCaptcha, string sessionCaptcha, string userHostAddress)
        {
            return inputCaptcha == sessionCaptcha || IsValidSecretCaptcha(inputCaptcha, userHostAddress);
        }

        public byte[] GenerateImageBuffer(string captcha, int width, int height, bool isComplex = false)
        {
            var RedNumber = _appSetting.RedNumberRGB;
            var GreenNumber = _appSetting.GreenNumberRGB;
            var BlueNumber = _appSetting.BlueNumberRGB;

            byte[] buffer = null;

            using (var memoryStream = new MemoryStream())
            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var random = new Random();

                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                graphics.Clear(Color.FromArgb(RedNumber, GreenNumber, BlueNumber));

                if (isComplex)
                {
                    DrawComplexImage(captcha, width, height, graphics, random);
                }
                else
                {
                    // Add captcha
                    var font = new Font("Montserrat", Number.Fourteen);
                    graphics.DrawString(captcha, font, Brushes.White, random.Next(Number.Two, Number.Ten), random.Next(Number.Zero, Number.Two));
                }

                for (int i = 0; i < Number.Fifty; i++)
                {
                    int x = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                bitmap.Save(memoryStream, ImageFormat.Png);

                buffer = memoryStream.GetBuffer();
            }

            return buffer;
        }

        private void DrawComplexImage(string captcha, int width, int height, Graphics graphics, Random random)
        {
            var rectangleF = new RectangleF(0, 0, width, height);
            var brush = new HatchBrush(
                hatchStyleList[random.Next(hatchStyleList.Length - 1)],
                CreateRandomColor(random, Number.OneFiveZero, Number.TwoDoubleFive),
                Color.White);
            graphics.FillRectangle(brush, rectangleF);
            var matrix = new Matrix();

            for (var i = 0; i <= captcha.Length - 1; i++)
            {
                matrix.Reset();
                int charLength = captcha.Length;
                int x = (width / (charLength + 1) * i) + 5;
                int y = random.Next(Number.Zero, Number.Two);

                //Rotate text Random
                matrix.RotateAt(random.Next(Number.NegativeTwenty, Number.Fifteen), new PointF(x, y));
                graphics.Transform = matrix;

                //Draw the letters with Randon Font Type, Size and Color
                graphics.DrawString(
                    //Text
                    captcha.Substring(i, 1),
                    //Random Font Name and Style
                    new Font(fontNameList[random.Next(fontNameList.Length - 1)], fontEmSizeList[random.Next(fontEmSizeList.Length - 1)], fontStyleList[random.Next(fontStyleList.Length - 1)]),
                    //Random Color (Darker colors RGB 0 to 100)
                    new SolidBrush(CreateRandomColor(random, Number.Zero, Number.OneHundred)),
                    x,
                    y);
                graphics.ResetTransform();
            }
        }


        private bool IsValidSecretCaptcha(string inputCaptcha, string userHostAddress)
        {
            var isAllowAutomationCaptchaCode = _appSetting.GetValue("AllowAutomationCaptchaCode") == "1";
            var secretCaptcha = _appSetting.GetValue("SecretCaptcha");

            return isAllowAutomationCaptchaCode &&
                !string.IsNullOrWhiteSpace(secretCaptcha) &&
                inputCaptcha == secretCaptcha &&
                IsValidHostAddress(userHostAddress);
        }

        private bool IsValidHostAddress(string userHostAddress)
        {
            var forwardedIP = RequestHelper.GetClientIPAddress();

            LogHelper.WriteInfo($"Client IP {userHostAddress}(forwarded IP {forwardedIP}) using static Captcha to access!");

            var companyIpRegex = _appSetting.GetValue("CompanyIP");

            return _appSetting.IsLocal || Regex.IsMatch(forwardedIP, companyIpRegex) || Regex.IsMatch(userHostAddress, companyIpRegex);
        }

        private static Color CreateRandomColor(Random random, int min, int max)
        {
            var red = random.Next(min, max);
            var green = random.Next(min, max);
            var blue = random.Next(min, max);

            return Color.FromArgb(red, green, blue);
        }
    }
}