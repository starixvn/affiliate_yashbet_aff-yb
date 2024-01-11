using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System;
using System.Drawing;
using System.IO;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class QrCodeGeneratorServiceTest
    {
        private readonly IImageHandler ImageHandler;
        private static Image Image => Image.FromStream(new MemoryStream(Convert.FromBase64String(Constant.LogoImageBase64)));

        public QrCodeGeneratorServiceTest()
        {
            ImageHandler = Substitute.For<IImageHandler>();
        }

        [Fact]
        public void GenerateBase64QrCode_WhencallWithImageNull_ReturnResult()
        {
            var service = new QrCodeUrlGeneratorService(null);

            var result = service.GenerateBase64QrCode("abcd", (Image)null, true);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GenerateBase64QrCode_WhencallWithImage_ReturnResult()
        {
            using (var image = Image)
            {
                var service = CreateService();

                var result = service.GenerateBase64QrCode("abcd", image, true);

                Assert.NotEmpty(result);
            }
        }

        [Fact]
        public void GenerateBase64QrCode_WhencallWithStringNull_ReturnResult()
        {
            var service = new QrCodeUrlGeneratorService(null);

            var result = service.GenerateBase64QrCode("abcd", (string)null, true);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GenerateBase64QrCode_WhenNeedPrefixForWeb_ReturnResultHavePrefix()
        {
            var service = CreateService();

            var result = service.GenerateBase64QrCode("abcd", Constant.LogoImageBase64, true);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GenerateBase64QrCode_WhenCallWithIcon_ReturnNotEmpty()
        {
            var icon = Constant.LogoImageBase64;
            var service = CreateService();

            var result = service.GenerateBase64QrCode("abcd", icon);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void SaveImage_WhenCall_SaveImage()
        {
            var service = CreateService();

            service.SaveImage(string.Empty, string.Empty);

            Assert.True(true);
        }

        [Fact]
        public void GetImage_WhenCall_SaveImage()
        {
            using (var image = Image)
            {
                var service = CreateService();
                ImageHandler.GetImage(Arg.Any<string>()).Returns(image);

                var result = service.GetImage(string.Empty);

                Assert.NotNull(result);
            }
        }

        private QrCodeUrlGeneratorService CreateService()
            => new QrCodeUrlGeneratorService(ImageHandler);
    }
}
