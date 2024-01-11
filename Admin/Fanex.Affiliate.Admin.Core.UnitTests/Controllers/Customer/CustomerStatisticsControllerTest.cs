using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Customer;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class CustomerStatisticsControllerTest
    {
        [Fact]
        public void GetCustomerStatistics_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new CustomerStatisticsParam { AffId = 1271808 };
            CustomerStatisticsParam passedParam = null;
            var customerStatisticsService = new Mock<ICustomerStatisticsService>();
            customerStatisticsService.Setup(s => s.GetCustomerStatistics(param)).Returns(new CustomerStatistics { NoOfSubAffByMonth = 123 }).Callback<CustomerStatisticsParam>(p => passedParam = p);
            var controller = CreateCustomerStatisticsController(customerStatisticsService: customerStatisticsService.Object);

            // Action
            var result = controller.GetCustomerStatistics(param) as JsonNetResult;
            var customerStatistics = result.Data as CustomerStatistics;

            // Assert
            Assert.Equal(123, customerStatistics.NoOfSubAffByMonth);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetCustomerStatistics_WhenCallWithInvalidParam_ReturnNull()
        {
            // Arrange
            var controller = CreateCustomerStatisticsController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetCustomerStatistics(new CustomerStatisticsParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private CustomerStatisticsController CreateCustomerStatisticsController(
            ISessionService sessionService = null,
            ICustomerStatisticsService customerStatisticsService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeCustomerStatisticsService = customerStatisticsService ?? Mock.Of<ICustomerStatisticsService>();

            return new CustomerStatisticsController(fakeSessionService, fakeCustomerStatisticsService);
        }
    }
}
