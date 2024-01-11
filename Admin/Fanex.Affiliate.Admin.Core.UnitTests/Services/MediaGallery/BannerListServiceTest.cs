using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Repository.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.MediaGallery
{
    public class BannerListServiceTest
    {
        private const string ErrorMessage = "Sorry! We just support these file types : .jpg, .gif, .png, .jpeg <br />";

        private static MemoryStream MemoryStream => new MemoryStream(Convert.FromBase64String(Constant.ImageBase64));


        [Fact]
        public void GetBannerList_WhenCallWithNotSubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new BannerParam();
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(b => b.GetBannerList(param)).Returns(new EntityResult<BannerInfo> { MainData = new List<BannerInfo> { new BannerInfo() }, TotalItem = 1 });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetBannerList(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetBannerList_WhenCallWithHaveSubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new BannerParam { Category = "Casino", SubCategory = "Casino Bonus" };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(b => b.GetBannerList(param))
                .Returns(new EntityResult<BannerInfo> { MainData = new List<BannerInfo> { new BannerInfo() }, TotalItem = 1 });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetBannerList(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetBannerList_WhenCallWithHaveEmptySubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new BannerParam { Category = "Casino" };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(b => b.GetBannerList(It.IsAny<BannerParam>()))
                .Returns(new EntityResult<BannerInfo> { MainData = new List<BannerInfo> { new BannerInfo() }, TotalItem = 1 });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetBannerList(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetBannerDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var selectedBannerId = 123;
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(selectedBannerId, 1)).Returns(new BannerInfo { BannerId = 123 });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetBannerDetail(selectedBannerId, 1);

            // Assert
            Assert.Equal(selectedBannerId, result.BannerId);
        }

        [Fact]
        public void InsertBanner_WhenCallWithInvalidCurrency_ReturnError()
        {
            // Arrange
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "Please select Currency." });
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream(), fileName: "abc.pdf")) }, new InsertUpdateMultipleBannersParam { UserType = 1 });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Please select Currency.", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithInvalidFile_ReturnError()
        {
            // Arrange
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = ErrorMessage });
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream(), fileName: "abc.pdf")) }, new InsertUpdateMultipleBannersParam { Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("File abc.pdf is incorrect.<br />" + ErrorMessage, result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithInputFileNamesMaxLenght_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "123test123test123test123test123test123test123test123test.png", Currencies = "USD" };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream())) }, param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("The maximum length of file name is 50. Please choose another name File Name 1.<br />", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithSpecialCharacter_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "abc*.png", Currencies = "USD" };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream())) }, param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("A File Name can't contain any of the following characters: \\/:*?\"<>|#%&+", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithDuplicateFileName_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png|abc.png", Currencies = "USD" };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream())) }, param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("File Name is unique, please enter other file name!", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithInvalidInputFileNames_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "test.pdf|abc.png", Currencies = "USD" };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.SetupSequence(s => s.GetExtension(It.IsAny<string>())).Returns(".pdf").Returns(".png");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream())) }, param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("File Name 1 is incorrect.<br />Sorry! We just support these file types : .jpg, .gif, .png, .jpeg<br />", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithInvalidFileMaxLenght_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream(), fileName: "123test123test123test123test123test123test123test123test.png")) }, param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("The maximum length of file name is 50. Please choose another name for File Name 1 or select another File.<br />", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithExistedFile_ReturnError()
        {
            // Arrange
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "The file name already exists." });
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream(), fileName: "abc.png")) }, new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("abc.png, This file name(s) belong to existing banner(s). Please change the file name.", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithCanNotAccessFolder_ReturnError()
        {
            // Arrange
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.ErrCanNotAccessFolder, Message = "Can not access folder." });
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: new MemoryStream(), fileName: "abc.png")) }, new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Can not access folder.", result.Message);
        }

        [Fact]
        public void InsertBanner_WhenCallWithValidFile_InsertError()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(new string[] { "abc" });
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.InsertBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: MemoryStream, fileName: "abc.png")) }, new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void InsertBanner_WhenCallWithValidFile_InsertSuccessfully()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(new string[] { "abc" });
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.InsertBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo());
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.InsertBanner(new MockingFile[1] { (new MockingFile(stream: MemoryStream, fileName: "abc.png")) }, new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithInvalidCurrency_ReturnError()
        {
            // Arrange
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "Please select Currency." });
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(), new InsertUpdateMultipleBannersParam { UserType = 1 });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Please select Currency.", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithSpecialCharacter_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "abc*.png", Currencies = "USD", UserType = 2 };
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var service = CreateBannerListService(bannerValidationService: bannerValidationService.Object, systemFile: systemFile.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(stream: new MemoryStream()), param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Sub-Affiliate", param.Folder);
            Assert.Equal("A File Name can't contain any of the following characters: \\/:*?\"<>|#%&+", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithInvalidInputFileName_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "test.pdf", Currencies = "USD" };
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".pdf");
            var service = CreateBannerListService(systemFile: systemFile.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(), param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Players", param.Folder);
            Assert.Equal("Sorry! We just support these file types : .jpg, .gif, .png, .jpeg<br />", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithInvalidFileName_ReturnError()
        {
            // Arrange
            var param = new InsertUpdateMultipleBannersParam { InputFileNames = "", Currencies = "USD" };
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var service = CreateBannerListService(systemFile: systemFile.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(stream: new MemoryStream(), fileName: "123test123test123test123test123test123test123test123test.png"), param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("The maximum length of file name is 50. Please either choose another file name or select another file.", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithExistedFile_ReturnError()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".gif");
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.ErrTheFileNameAlreadyExists, Message = "The file name already exists." });
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "abc.gif" });
            var service = CreateBannerListService(bannerListRepository.Object, null, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(), new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("The file name already exists.", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithInvalidFile_ReturnError()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.ErrTheFileNameAlreadyExists });
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode, Message = "Invalid File" });
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "CategoryMedia/Players/abc.png" });
            bannerListRepository.Setup(s => s.UpdateBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });
            var service = CreateBannerListService(bannerListRepository.Object, null, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(stream: MemoryStream, fileName: "abc.png"), new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Invalid File", result.Message);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithValidFile_UpdateError()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(new string[] { "abc" });
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.ErrTheFileNameAlreadyExists });
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "CategoryMedia/Players/abc.png" });
            bannerListRepository.Setup(s => s.UpdateBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(stream: MemoryStream, fileName: "abc.png"), new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithValidFile_UpdateSuccess()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(new string[] { "abc" });
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo { Code = ErrCode.ErrTheFileNameAlreadyExists });
            bannerValidationService.Setup(b => b.ValidateFile(It.IsAny<string>(), It.IsAny<int>())).Returns(new ResultInfo());
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "CategoryMedia/Players/abc.png" });
            bannerListRepository.Setup(s => s.UpdateBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo());
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(new MockingFile(stream: MemoryStream, fileName: "abc.png"), new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithValidParam_UpdateSuccess()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "CategoryMedia/Players/abc.png" });
            bannerListRepository.Setup(s => s.UpdateBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo());
            var service = CreateBannerListService(bannerListRepository.Object, null, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(null, new InsertUpdateMultipleBannersParam { InputFileNames = "abc.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateBanner_WhenCallWithValidParamHasMoveFile_UpdateSuccess()
        {
            // Arrange
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.GetExtension(It.IsAny<string>())).Returns(".png");
            systemFile.Setup(s => s.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var bannerValidationService = new Mock<IBannerValidationService>();
            bannerValidationService.Setup(b => b.CheckFileInFolder(It.IsAny<string>(), It.IsAny<string>())).Returns(new ResultInfo());
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "CategoryMedia/abc.png" });
            bannerListRepository.Setup(s => s.UpdateBanner(It.IsAny<InsertUpdateMultipleBannersParam>())).Returns(new ResultInfo());
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(new string[] { "abc" });
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object, bannerValidationService.Object);

            // Action
            var result = service.UpdateBanner(null, new InsertUpdateMultipleBannersParam { InputFileNames = "xyz.png", UserType = 1, Currencies = "USD" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteBanners_WhenCall_DeleteSuccessfully()
        {
            // Arrange
            string[] bannerFolders = { "abc" };
            var param = new DeleteBannerParam { BannerIdList = "123", UserType = 1 };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "abc.gif" });
            bannerListRepository.Setup(s => s.DeleteBanners(param)).Returns(new ResultInfo());
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.BannerFolders).Returns(bannerFolders);
            var systemFile = new Mock<ISystemFile>();
            systemFile.Setup(s => s.DeleteFile(It.IsAny<string>(), It.IsAny<string>()));
            var service = CreateBannerListService(bannerListRepository.Object, appSetting.Object, systemFile.Object);

            // Action
            var result = service.DeleteBanners(param);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteBanners_WhenCall_DeleteIsFailed()
        {
            // Arrange
            var param = new DeleteBannerParam { BannerIdList = "123", UserType = 1 };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(s => s.GetBannerDetail(It.IsAny<int>(), 1)).Returns(new BannerInfo { FileName = "abc.gif" });
            bannerListRepository.Setup(s => s.DeleteBanners(param)).Returns(new ResultInfo { Code = 999, Message = "Delete is failed" });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.DeleteBanners(param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Delete is failed", result.Message);
        }

        [Fact]
        public void GetSizeList_WhenCallWithNotSubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new SizeParam();
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(t => t.GetSizeList(It.IsAny<SizeParam>())).Returns(new List<MediaGalleryFilterItem> { new MediaGalleryFilterItem { Id = 456, Name = "320x250" } });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetSizeList(param);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetSizeList_WhenCallWithHaveSubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new SizeParam { Category = "Casino", SubCategory = "Casino Bonus" };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(t => t.GetSizeList(It.IsAny<SizeParam>())).Returns(new List<MediaGalleryFilterItem> { new MediaGalleryFilterItem { Id = 456, Name = "320x250" } });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetSizeList(param);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetSizeList_WhenCallWithHaveEmptySubCategory_ReturnCorrectData()
        {
            // Arrange
            var param = new SizeParam { Category = "Casino" };
            var bannerListRepository = new Mock<IBannerListRepository>();
            bannerListRepository.Setup(t => t.GetSizeList(It.IsAny<SizeParam>())).Returns(new List<MediaGalleryFilterItem> { new MediaGalleryFilterItem { Id = 456, Name = "320x250" } });
            var service = CreateBannerListService(bannerListRepository.Object);

            // Action
            var result = service.GetSizeList(param);

            // Assert
            Assert.Single(result);
        }

        private static BannerListService CreateBannerListService(
            IBannerListRepository bannerListRepository = null,
            IAppSetting appSetting = null,
            ISystemFile systemFile = null,
            IBannerValidationService bannerValidationService = null)
        {
            var fakeBannerListRepository = bannerListRepository ?? Mock.Of<IBannerListRepository>();
            var fakeAppSetting = appSetting ?? Mock.Of<IAppSetting>();
            var fakeSystemFile = systemFile ?? Mock.Of<ISystemFile>();
            var fakeBannerValidationService = bannerValidationService ?? Mock.Of<IBannerValidationService>();

            return new BannerListService(fakeBannerListRepository, fakeAppSetting, fakeSystemFile, fakeBannerValidationService);
        }
    }
}
