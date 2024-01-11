using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Services.MediaGallery;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.MediaGallery
{
    public class TextListServiceTest
    {
        private const string SpKeyTextSel = "Aff_Admin_Text_Sel";
        private const string SpKeyTextIns = "Aff_Admin_Text_Ins";
        private const string SpKeyTextUpd = "Aff_Admin_Text_Upd";
        private const string SpKeyTextDel = "Aff_Admin_Text_Del";

        [Fact]
        public void GetTextList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTextList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb(SpKeyTextSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTextList(new TextParam());

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public void GetTextList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb(SpKeyTextSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTextList(new TextParam());

            // Assert
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public void GetTextDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(true);

            ReaderHelper.MapReaderData(dataReader, GetTextList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyTextSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTextDetail(123, 1);

            // Assert
            Assert.Equal(123, result.TextId);
        }

        [Fact]
        public void GetTextDetail_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyTextSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTextDetail(123, 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertText_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyTextIns, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertText(new InsertUpdateTextParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertText_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert Text is failed.";
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyTextIns, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertText(new InsertUpdateTextParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateText_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyTextUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateText(new InsertUpdateTextParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateText_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update Text is failed.";
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyTextUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateText(new InsertUpdateTextParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteTexts_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyTextDel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteTexts(new DeleteTextParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteTexts_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Delete Text(s) is failed.";
            var service = new Mock<TextListService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyTextDel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteTexts(new DeleteTextParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetTextList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TextID"] = 123,
                ["Message"] = "test",
                ["Category"] = "Basketball",
                ["Language"] = "English",
                ["Currency"] = "EURO",
                ["DeviceType"] = 1,
                ["TargetId"] = 1
            };
        }
    }
}
