using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;

namespace Fanex.Affiliate.Admin.Core.Services.Customer
{
    public interface ICustomerStatisticsService
    {
        CustomerStatistics GetCustomerStatistics(CustomerStatisticsParam param);
    }
}
