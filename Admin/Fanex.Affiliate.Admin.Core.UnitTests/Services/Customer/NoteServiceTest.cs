using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class NoteServiceTest
    {
        [Fact]
        public void GetNoteList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetNoteList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateNote_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetNoteList(new AffiliatePagingParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("11/22/2019 00:00:00", item.LastModifyText);
        }

        [Fact]
        public void GetNoteList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetNoteListHasUpdatedDateIsNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateNote_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetNoteList(new AffiliatePagingParam());
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("N/A", item.LastModifyText);
        }

        [Fact]
        public void GetNoteList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateNote_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetNoteList(new AffiliatePagingParam());

            // Assert
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public void GetNoteContentById_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(true);

            ReaderHelper.MapReaderData(dataReader, GetNoteList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateNote_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetNoteContentById(1271808, 37);

            // Assert
            Assert.Equal(37, result.NoteId);
        }

        [Fact]
        public void GetNoteContentById_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_AffiliateNote_Detail_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetNoteContentById(1271808, 37);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertNote_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateNote_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertNote(new InsertUpdateNoteParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertNote_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Insert Note is failed.";
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateNote_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertNote(new InsertUpdateNoteParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateNote_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateNote_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateNote(new InsertUpdateNoteParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateNote_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update Note is failed.";
            var service = new Mock<NoteService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_AffiliateNote_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateNote(new InsertUpdateNoteParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetNoteList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["NoteId"] = 37,
                ["Note"] = "abc",
                ["UpdatedName"] = "sa",
                ["UpdatedDate"] = new DateTime(2019, 11, 22)
            };
        }

        private IDictionary<string, object> GetNoteListHasUpdatedDateIsNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["NoteId"] = 37,
                ["Note"] = "abc",
                ["UpdatedName"] = "sa",
                ["UpdatedDate"] = DBNull.Value
            };
        }
    }
}
