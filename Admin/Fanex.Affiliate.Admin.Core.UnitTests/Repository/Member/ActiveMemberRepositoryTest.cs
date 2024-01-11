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
    public class ActiveMemberRepositoryTest
    {
        [Fact]
        public void GetAllMembers_WhenCall_ReturnCorrectInactiveMemberData()
        {
            // Arrange
            var repository = new Mock<ActiveMemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetInactivatedMemberList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("abc");
            repository.Setup(r => r.GetObjectDb("Aff_Admin_ActiveMember_CheckStatus", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAllMembers("B2Caff226,abc");

            // Assert
            Assert.Single(result.InactiveMemberList);
            Assert.Equal("abc", result.MemberNotExist);
        }

        [Fact]
        public void GetAllMembers_WhenCall_ReturnCorrectActiveMemberData()
        {
            // Arrange
            var repository = new Mock<ActiveMemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetActivatedMemberList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("abc");
            repository.Setup(r => r.GetObjectDb("Aff_Admin_ActiveMember_CheckStatus", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAllMembers("B2Caff226,abc");
            var item = result.ActivedMemeberList.FirstOrDefault();

            // Assert
            Assert.Single(result.ActivedMemeberList);
            Assert.Equal("abc", result.MemberNotExist);
            Assert.Equal("10/23/2019 00:00:00", item.CreatedDateText);
        }

        [Fact]
        public void GetAllMembers_WhenCall_ReturnActiveMemberHasCreatedDateIsNull()
        {
            // Arrange
            var repository = new Mock<ActiveMemberRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetActivatedMemberListHasCreatedDateIsNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("abc");
            repository.Setup(r => r.GetObjectDb("Aff_Admin_ActiveMember_CheckStatus", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAllMembers("B2Caff226,abc");
            var item = result.ActivedMemeberList.FirstOrDefault();

            // Assert
            Assert.Single(result.ActivedMemeberList);
            Assert.Equal("abc", result.MemberNotExist);
            Assert.Null(item.CreatedDateText);
        }

        private IDictionary<string, object> GetInactivatedMemberList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["UserName"] = "B2Caff226",
                ["FirstName"] = "B2C",
                ["LastName"] = "Aff"
            };
        }

        private IDictionary<string, object> GetActivatedMemberList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["IBCCustID"] = 123,
                ["UserName"] = "B2Caff226",
                ["IBCUserName"] = "B2Caff226",
                ["FirstName"] = "B2C",
                ["LastName"] = "Aff",
                ["CreatedDate"] = new DateTime(2019, 10, 23)
            };
        }

        private IDictionary<string, object> GetActivatedMemberListHasCreatedDateIsNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["IBCCustID"] = 123,
                ["UserName"] = "B2Caff226",
                ["IBCUserName"] = "B2Caff226",
                ["FirstName"] = "B2C",
                ["LastName"] = "Aff",
                ["CreatedDate"] = DBNull.Value
            };
        }
    }
}
