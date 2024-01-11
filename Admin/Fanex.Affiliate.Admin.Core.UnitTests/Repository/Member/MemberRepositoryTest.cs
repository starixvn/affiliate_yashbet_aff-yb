using Fanex.Affiliate.Admin.Core.MetaModels.Member;
using Fanex.Affiliate.Admin.Core.Repository.Member;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Member
{
    public class MemberRepositoryTest
    {
        private const string SpKeyMemberListSel = "Aff_Admin_MemberList_Sel";

        [Fact]
        public void GetMemberList_WhenCallWithSpKeyBannerSel_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetMemberList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(SpKeyMemberListSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMemberList(new MemberParam());
            var memberItem = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(1, result.TotalItem);
            //Assert.Equal("10/10/2019 00:00:00", memberItem.CreatedDateText);
        }

        [Fact]
        public void GetMemberList_WhenCallWithSpKeyBannerSel_ReturnDataHasCreatedDateIsNull()
        {
            // Arrange
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetMemberListHasCreatedDateIsNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            repository.Setup(r => r.GetObjectDb(SpKeyMemberListSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMemberList(new MemberParam());
            var memberItem = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal(1, result.TotalItem);
            //Assert.Null(memberItem.CreatedDateText);
        }

        [Fact]
        public void GetMemberList_WhenCallWithSpKeyBannerSel_ReturnNullData()
        {
            // Arrange
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            repository.Setup(r => r.GetObjectDb(SpKeyMemberListSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMemberList(new MemberParam());

            // Assert
            Assert.Empty(result.MainData);
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public void GetMemberDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetMemberList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_Player_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMemberDetail(123);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMemberDetail_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb("Aff_Admin_Player_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetMemberDetail(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateMember_WhenCall_UpdateSuccessfully()
        {
            // Arrange
            var spKey = "Aff_Admin_Member_RemoveStatus_Upd";
            var repository = new Mock<MemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateMember(new UpdateMemberParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private IDictionary<string, object> GetMemberList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustID"] = 123,
                ["UserName"] = "B2Caff226",
                ["IBCUserName"] = "B2Caff226",
                ["FirstName"] = "B2C",
                ["LastName"] = "Aff",
                ["CurrBalance"] = 123m,
                ["RegisteredDate"] = new DateTime(2019, 10, 10),
                ["LastAction"] = new DateTime(2019, 10, 10),
                ["IsInclude"] = true,
                ["IsRemoved"] = false,
                ["AffUserName"] = "B2CAFF226",
            };
        }

        private IDictionary<string, object> GetMemberListHasCreatedDateIsNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustID"] = 123,
                ["UserName"] = "B2Caff226",
                ["IBCUserName"] = "B2Caff226",
                ["FirstName"] = "B2C",
                ["LastName"] = "Aff",
                ["CurrBalance"] = -123m,
                ["RegisteredDate"] = DBNull.Value,
                ["LastAction"] = DBNull.Value,
                ["IsInclude"] = true,
                ["IsRemoved"] = false,
                ["AffUserName"] = "B2CAFF226",
            };
        }
    }
}
