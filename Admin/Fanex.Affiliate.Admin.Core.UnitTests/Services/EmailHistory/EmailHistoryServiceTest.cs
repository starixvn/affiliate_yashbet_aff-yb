using Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory;
using Fanex.Affiliate.Admin.Core.Services.EmailHistory;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.EmailHistory
{
    public class EmailHistoryServiceTest
    {
        [Fact]
        public void GetEmailHistoryList_WhenCall_ReturnCorrectEmailInfoList()
        {
            // Arrange
            var service = new Mock<EmailHistoryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedEmailList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_EmailHistory_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetEmailHistoryList(new EmailHistoryParam());
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("04/15/2020 00:00:00", item.SentDateText);
        }

        [Fact]
        public void GetEmailHistoryList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<EmailHistoryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_EmailHistory_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetEmailHistoryList(new EmailHistoryParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        [Fact]
        public void GetEmailHistoryDetail_WhenCall_ReturnCorrectEmailInfo()
        {
            // Arrange
            var service = new Mock<EmailHistoryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedEmailList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_EmailHistory_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetEmailHistoryDetail(123);

            // Assert
            Assert.Equal(123, result.EmailId);
        }

        [Fact]
        public void GetEmailHistoryDetail_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<EmailHistoryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_EmailHistory_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetEmailHistoryDetail(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeleteEmailHistory_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var service = new Mock<EmailHistoryService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_EmailHistory_Del", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteEmailHistory(new DeleteEmailHistoryParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private IDictionary<string, object> GetExpectedEmailList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Affname"] = "B2CAff226",
                ["MID"] = 123,
                ["Sender"] = "123",
                ["Subject"] = "vi-VN",
                ["Email"] = "abc@gmail.com",
                ["CreatedDate"] = new DateTime(2020, 04, 15),
                ["Content"] = "abc"
            };
        }
    }
}
