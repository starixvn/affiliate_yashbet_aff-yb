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
    public class CustomerMemberServiceTest
    {
        [Fact]
        public void GetCustomerMemberList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<CustomerMemberService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedMemberList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_Member_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerMember(new CustomerMemberParam());
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("04/15/2020 00:00:00", item.RegisteredDateString);
        }

        [Fact]
        public void GetCustomerMemberList_WhenCallWithCanViewMemberEmail_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<CustomerMemberService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedMemberList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_Member_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerMember(new CustomerMemberParam { CanViewMemberEmail = true });
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("04/15/2020 00:00:00", item.RegisteredDateString);
        }

        [Fact]
        public void GetCustomerMemberList_WhenCallWithCanViewMemberEmail_ReturnCorrectDataHasNull()
        {
            // Arrange
            var service = new Mock<CustomerMemberService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedMemberListHasNull());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_Member_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerMember(new CustomerMemberParam { CanViewMemberEmail = true });
            var item = result.MainData.SingleOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Null(item.RegisteredDateString);
        }

        [Fact]
        public void GetCustomerMemberList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<CustomerMemberService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(re => re.GetObjectDb("Aff_Admin_Member_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerMember(new CustomerMemberParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetExpectedMemberList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["UserName"] = "N88AFF001",
                ["FirstName"] = "123",
                ["LastName"] = "abc",
                ["Email"] = "abc@gmail.com",
                ["RegisteredDate"] = new DateTime(2020, 04, 15),
                ["Currency"] = "USD"
            };
        }

        private IDictionary<string, object> GetExpectedMemberListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["UserName"] = "N88AFF001",
                ["FirstName"] = DBNull.Value,
                ["LastName"] = DBNull.Value,
                ["Email"] = DBNull.Value,
                ["RegisteredDate"] = DBNull.Value,
                ["Currency"] = DBNull.Value
            };
        }
    }
}
