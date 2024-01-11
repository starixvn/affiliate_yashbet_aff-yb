using Fanex.Affiliate.Admin.Core.MetaModels.Message;
using Fanex.Affiliate.Admin.Core.Services.Message;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Service.Message
{
    public class MessageServiceTest
    {
        private const string SpKeyHotNewsSel = "Aff_Admin_Message_Sel";
        private const string SpKeyHotNewsIns = "Aff_Admin_Message_Ins";
        private const string SpKeyHotNewsUpd = "Aff_Admin_Message_Upd";
        private const string SpKeyHotNewsDel = "Aff_Admin_Message_Del";

        [Fact]
        public void GetMessageList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetMessageList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb(SpKeyHotNewsSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetMessageList(new MessageParam());
            var messageInfo = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(1, result.TotalItem);
            Assert.Equal("09/18/2019 00:00:00", messageInfo.PostedDateText);
            Assert.Equal("09/19/2019 00:00:00", messageInfo.UpdatedDateText);
        }

        [Fact]
        public void GetMessageList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetMessageList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb(SpKeyHotNewsSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetMessageList(new MessageParam());
            var messageInfo = result.MainData.FirstOrDefault();
            messageInfo.UpdatedDate = null;
            messageInfo.PostedDate = null;

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(1, result.TotalItem);
            Assert.Null(messageInfo.PostedDateText);
            Assert.Null(messageInfo.UpdatedDateText);
        }

        [Fact]
        public void GetMessageList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb(SpKeyHotNewsSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetMessageList(new MessageParam());
            var messageInfo = result.MainData.FirstOrDefault();

            // Assert
            Assert.Equal(0, result.TotalItem);
            Assert.Equal("01/01/0001 00:00:00", messageInfo.UpdatedDateText);
        }

        [Fact]
        public void GetMessageDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(true);

            ReaderHelper.MapReaderData(dataReader, GetMessageList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyHotNewsSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetMessageDetail(37);

            // Assert
            Assert.Equal(37, result.MessageId);
        }

        [Fact]
        public void GetMessageDetail_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb(SpKeyHotNewsSel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetMessageDetail(37);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertMessage_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsIns, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertMessage(new InsertUpdateMessageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertMessage_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Insert Message is failed.";
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsIns, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertMessage(new InsertUpdateMessageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateMessage_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateMessage(new InsertUpdateMessageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateMessage_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update Message is failed.";
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateMessage(new InsertUpdateMessageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteMessages_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsDel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteMessages(new DeleteMessageParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteMessages_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Delete Message(s) is failed.";
            var service = new Mock<MessageService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb(SpKeyHotNewsDel, 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteMessages(new DeleteMessageParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetMessageList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["MessageId"] = 37,
                ["creatorname"] = "sa",
                ["PostedDate"] = new DateTime(2019, 09, 18),
                ["message_en"] = "test",
                ["Important"] = false,
                ["updatedname"] = "",
                ["UpdatedDate"] = new DateTime(2019, 09, 19)
            };
        }
    }
}
