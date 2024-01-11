using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Services.Customer;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class CustomerStatisticsServiceTest
    {
        [Fact]
        public void GetCustomerStatistics_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<CustomerStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(true);

            ReaderHelper.MapReaderData(dataReader, GetCustomerStatistics());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_Statistics_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerStatistics(new CustomerStatisticsParam { MonthDate = new DateTime() });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetCustomerStatistics_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<CustomerStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_Statistics_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetCustomerStatistics(new CustomerStatisticsParam { MonthDate = new DateTime() });

            // Assert
            Assert.Null(result);
        }

        private IDictionary<string, object> GetCustomerStatistics()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["SignupsByMonth"] = 0,
                ["FDMByMonth"] = 0,
                ["SignupsTotal"] = 0,
                ["FDMTotal"] = 0,
                ["ActivePlayersByMonth"] = 0,
                ["DepositByMonth"] = 0,
                ["DepositTotal"] = 0,
                ["TurnoverByMonth"] = 0,
                ["TurnoverTotal"] = 0,
                ["GgrByMonth"] = 0,
                ["GgrTotal"] = 0,
                ["TotalClickByMonth"] = 0,
                ["UniqueClickByMonth"] = 0,
                ["UniqueClickPCByMonth"] = 0,
                ["TotalClickPCByMonth"] = 0,
                ["UniqueClickMobileByMonth"] = 0,
                ["TotalClickMobileByMonth"] = 0,
                ["NoOfSubAffByMonth"] = 0,
                ["NoOfSubAffTotal"] = 0
            };
        }
    }
}
