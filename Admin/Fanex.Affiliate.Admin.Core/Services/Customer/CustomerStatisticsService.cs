using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Repository;
using System;

namespace Fanex.Affiliate.Admin.Core.Services.Customer
{
    public class CustomerStatisticsService : BaseRepository, ICustomerStatisticsService
    {
        public CustomerStatistics GetCustomerStatistics(CustomerStatisticsParam param)
        {
            const string SpKey = "Aff_Admin_Statistics_Sel";
            CustomerStatistics result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", param.AffId);
                objectDb.SetParameterValue("MonthDate", param.MonthDate.Value);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new CustomerStatistics
                        {
                            NewSignUpByMonth = Convert.ToInt32(reader["SignupsByMonth"]),
                            NewFdmByMonth = Convert.ToInt32(reader["FDMByMonth"]),
                            NewSignUpTotal = Convert.ToInt32(reader["SignupsTotal"]),
                            NewFdmTotal = Convert.ToInt32(reader["FDMTotal"]),
                            ActivePlayerByMonth = Convert.ToInt32(reader["ActivePlayersByMonth"]),
                            DepositByMonth = Convert.ToDecimal(reader["DepositByMonth"]),
                            DepositTotal = Convert.ToDecimal(reader["DepositTotal"]),
                            TurnoverByMonth = Convert.ToDecimal(reader["TurnoverByMonth"]),
                            TurnoverTotal = Convert.ToDecimal(reader["TurnoverTotal"]),
                            GgrByMonth = Convert.ToDecimal(reader["GgrByMonth"]),
                            GgrTotal = Convert.ToDecimal(reader["GgrTotal"]),
                            UniqueClickPcLaptopByMonth = Convert.ToInt32(reader["UniqueClickPCByMonth"]),
                            TotalClickPcLaptopByMonth = Convert.ToInt32(reader["TotalClickPCByMonth"]),
                            UniqueClickMobileByMonth = Convert.ToInt32(reader["UniqueClickMobileByMonth"]),
                            TotalClickMobileByMonth = Convert.ToInt32(reader["TotalClickMobileByMonth"]),
                            NoOfSubAffByMonth = Convert.ToInt32(reader["NoOfSubAffByMonth"]),
                            NoOfSubAffTotal = Convert.ToInt32(reader["NoOfSubAffTotal"])
                        };
                    }
                }
            }

            return result;
        }
    }
}