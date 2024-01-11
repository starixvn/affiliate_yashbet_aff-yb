using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class CustomerMemberControllerTest
    {
        [Fact]
        public void GetCustomerMember_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new CustomerMemberParam();
            CustomerMemberParam passedParam = null;
            var customerMemberService = new Mock<ICustomerMemberService>();
            customerMemberService.Setup(t => t.GetCustomerMember(param)).Returns(new EntityResult<CustomerMember> { MainData = new List<CustomerMember> { new CustomerMember() }, TotalItem = 1 }).Callback<CustomerMemberParam>(p => passedParam = p);
            var controller = CreateCustomerMemberController(customerMemberService: customerMemberService.Object);

            // Action
            var result = controller.GetCustomerMember(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<CustomerMember>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetCustomerMember_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateCustomerMemberController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetCustomerMember(new CustomerMemberParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private CustomerMemberController CreateCustomerMemberController(
            ISessionService sessionService = null,
            ICustomerMemberService customerMemberService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeCustomerMemberService = customerMemberService ?? Mock.Of<ICustomerMemberService>();

            return new CustomerMemberController(fakeSessionService, fakeCustomerMemberService);
        }
    }
}