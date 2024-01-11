using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.Utilities;
using Moq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.MediaGallery
{
    public class BannerValidationServiceTest
    {
        [Fact]
        public void ValidateFile_WhenCallWithInvalidFile_ReturnError()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".pdf");
            var service = CreateBannerValidationService(systemFile: systemFile.Object);

            // Action
            var result = service.ValidateFile("test.pdf", 3000);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Sorry! We just support these file types : .jpg, .gif, .png, .jpeg<br />", result.Message);
        }

        [Fact]
        public void ValidateFile_WhenCallWithContentLenght_ReturnError()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(s => s.ImageSize).Returns(30000);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerValidationService(appSetting.Object, systemFile.Object);

            // Action
            var result = service.ValidateFile("test.gif", 30003);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Your upload file size must be <= 3 MB.<br />", result.Message);
        }

        [Fact]
        public void ValidateFile_WhenCallWithValidParam_ExcuteSuccessful()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(s => s.ImageSize).Returns(30000);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerValidationService(appSetting.Object, systemFile.Object);

            // Action
            var result = service.ValidateFile("test.gif", 3000);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void CheckFileInFolder_WhenCall_CreateFolderFailed()
        {
            // Arrange
            string[] bannerFolders = { "test" };
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(s => s.BannerFolders).Returns(bannerFolders);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.CreateFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "Can not access the folder!" });
            var service = CreateBannerValidationService(appSetting.Object, systemFile.Object);

            // Action
            var result = service.CheckFileInFolder("abc", "test.gif");

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Can not access the folder!", result.Message);
        }

        [Fact]
        public void CheckFileInFolder_WhenCall_ReturnExistedFileError()
        {
            // Arrange
            string[] bannerFolders = { "test" };
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(s => s.BannerFolders).Returns(bannerFolders);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.CreateFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            systemFile.Setup(s => s.CheckExistedFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "The file has already existed!" });
            var service = CreateBannerValidationService(appSetting.Object, systemFile.Object);

            // Action
            var result = service.CheckFileInFolder("abc", "test.gif");

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("The file has already existed!", result.Message);
        }

        [Fact]
        public void CheckFileInFolder_WhenCallValidParam_ExcuteSuccessful()
        {
            // Arrange
            string[] bannerFolders = { "test" };
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(s => s.BannerFolders).Returns(bannerFolders);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.CreateFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            systemFile.Setup(s => s.CheckExistedFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            var service = CreateBannerValidationService(appSetting.Object, systemFile.Object);

            // Action
            var result = service.CheckFileInFolder("abc", "test.gif");

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private static BannerValidationService CreateBannerValidationService(
            IAppSetting appSetting = null,
            ISystemFile systemFile = null)
        {
            var fakeAppSetting = appSetting ?? Mock.Of<IAppSetting>();
            var fakeSystemFile = systemFile ?? Mock.Of<ISystemFile>();

            return new BannerValidationService(fakeAppSetting, fakeSystemFile);
        }
    }
}
