using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Repository.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.MediaGallery
{
    public class LandingPageRepositoryTest
    {
        [Fact]
        public void GetLandingPageList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetLandingPageList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_LandingPage_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetLandingPageList_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_LandingPage_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetLandingPageById_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(true);

            ReaderHelper.MapReaderData(dataReader, GetLandingPageList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_LandingPage_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageById(123);

            // Assert
            Assert.Equal(123, result.PageId);
        }

        [Fact]
        public void GetLandingPageById_WhenCall_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_LandingPage_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageById(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertLandingPage_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.InsertLandingPage(new InsertUpdateLandingPageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertLandingPage_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert landing page is failed.";
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.InsertLandingPage(new InsertUpdateLandingPageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateLandingPage_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateLandingPage(new InsertUpdateLandingPageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateLandingPage_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update landing page is failed.";
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateLandingPage(new InsertUpdateLandingPageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteLandingPage_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Del", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteLandingPage(new DeleteLandingPageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteLandingPage_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Delete Text(s) is failed.";
            var repository = new Mock<LandingPageRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb("Aff_Admin_LandingPage_Del", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteLandingPage(new DeleteLandingPageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetLandingPageList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["PageId"] = 123,
                ["PageName"] = "Basketball",
                ["PageCode"] = "Basketball"
            };
        }
    }
}
