using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Repository.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.MediaGallery
{
    public class LandingPageServiceTest
    {
        [Fact]
        public void GetLandingPageList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var landingPageRepository = new Mock<ILandingPageRepository>();
            landingPageRepository.Setup(l => l.GetLandingPageList()).Returns(new List<LandingPage> { new LandingPage() });
            var service = CreateLandingPageService(landingPageRepository.Object);

            // Action
            var result = service.GetLandingPageList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetLandingPageById_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var pageId = 123;
            int passedParam = 0;
            var landingPageRepository = new Mock<ILandingPageRepository>();
            landingPageRepository.Setup(l => l.GetLandingPageById(pageId)).Returns(new LandingPage { PageId = 123 }).Callback<int>(p => passedParam = p);
            var service = CreateLandingPageService(landingPageRepository.Object);

            // Action
            var result = service.GetLandingPageById(pageId);

            // Assert
            Assert.Equal(pageId, result.PageId);
            Assert.Equal(pageId, passedParam);
        }

        [Fact]
        public void InsertLandingPage_WhenCall_ExcuteCorrect()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageCode = "123" };
            InsertUpdateLandingPageParam passedParam = null;
            var landingPageRepository = new Mock<ILandingPageRepository>();
            landingPageRepository.Setup(l => l.InsertLandingPage(param)).Returns(new ResultInfo()).Callback<InsertUpdateLandingPageParam>(p => passedParam = p);
            var service = CreateLandingPageService(landingPageRepository.Object);

            // Action
            var result = service.InsertLandingPage(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void InsertLandingPage_WhenCall_ExcuteIncorrect()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageCode = "123@ abc" };
            var service = CreateLandingPageService();

            // Action
            var result = service.InsertLandingPage(param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Special characters are not allowed:\n the blank/empty space and ; / ? : @ = & \" < > # % { } | \\ ^ ~ [ ] `", result.Message);
        }

        [Fact]
        public void UpdateLandingPage_WhenCall_ExcuteCorrect()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageCode = "abc" };
            InsertUpdateLandingPageParam passedParam = null;
            var landingPageRepository = new Mock<ILandingPageRepository>();
            landingPageRepository.Setup(l => l.UpdateLandingPage(param)).Returns(new ResultInfo()).Callback<InsertUpdateLandingPageParam>(p => passedParam = p);
            var service = CreateLandingPageService(landingPageRepository.Object);

            // Action
            var result = service.UpdateLandingPage(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void UpdateLandingPage_WhenCall_ExcuteIncorrect()
        {
            // Arrange
            var param = new InsertUpdateLandingPageParam { PageCode = "abc@ 123" };
            var service = CreateLandingPageService();

            // Action
            var result = service.UpdateLandingPage(param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Special characters are not allowed:\n the blank/empty space and ; / ? : @ = & \" < > # % { } | \\ ^ ~ [ ] `", result.Message);
        }

        [Fact]
        public void DeleteLandingPage_WhenCall_ExcuteCorrect()
        {
            // Arrange
            var param = new DeleteLandingPageParam();
            DeleteLandingPageParam passedParam = null;
            var landingPageRepository = new Mock<ILandingPageRepository>();
            landingPageRepository.Setup(l => l.DeleteLandingPage(param)).Returns(new ResultInfo()).Callback<DeleteLandingPageParam>(p => passedParam = p);
            var service = CreateLandingPageService(landingPageRepository.Object);

            // Action
            var result = service.DeleteLandingPage(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        private static LandingPageService CreateLandingPageService(ILandingPageRepository landingPageRepository = null)
        {
            var fakeLandingPageRepository = landingPageRepository ?? Mock.Of<ILandingPageRepository>();

            return new LandingPageService(fakeLandingPageRepository);
        }
    }
}
