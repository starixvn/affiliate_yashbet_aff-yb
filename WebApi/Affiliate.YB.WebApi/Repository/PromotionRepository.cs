using Affiliate.YB.WebApi.DTO;
using Affiliate.YB.WebApi.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.WebApi.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        public async Task<IEnumerable<Promotion>> GetPromotionsAsync(DateTime fromDate, DateTime toDate)
        {
            var spKey = "API_Promotion_Get";
            using (var objectDb = ObjectDbFactory.CreateInstance(spKey))
            {
                using (var reader = await objectDb.ExecuteReaderAsync(new { FromDate = fromDate, ToDate = toDate }))
                {
                    var promos = new List<Promotion>();
                    while (reader.Read())
                    {
                        promos.Add(new Promotion
                        {
                            BonusCode = reader.GetVal<string>("BonusCode"),
                            CategoryId = reader.GetVal<int>("CategoryId"),
                            CreateTime = reader.GetVal<string>("CreateTime"),
                            Description = reader.GetVal<string>("Description"),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            ExpiryDays = reader.GetVal<int>("ExpiryDays"),
                            IsSpecial = reader.GetVal<bool>("IsSpecial"),
                            LastUpdateTime = reader.GetVal<DateTime?>("LastUpdateTime"),
                            LastUpdateUser = reader.GetVal<string>("LastUpdateUser"),
                            PeriodDay = reader.GetVal<int?>("PeriodDay"),
                            PeriodFrom = reader.GetVal<long>("PeriodFrom"),
                            PeriodTo = reader.GetVal<long>("PeriodTo"),
                            PeriodType = reader.GetVal<int?>("PeriodType"),
                            PromotionType = reader.GetVal<int>("PromotionType"),
                            RedeemHours = reader.GetVal<int?>("RedeemHours"),
                            SiteId = reader.GetVal<int>("SiteId"),
                            Sort = reader.GetVal<int>("Sort"),
                            SpecialCode = reader.GetVal<string>("SpecialCode"),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            Status = reader.GetVal<int>("Status"),
                            TpId = reader.GetVal<int?>("TpId"),
                            WalletType = reader.GetVal<int?>("WalletType")
                        });
                    }
                    return promos;
                }
            }
        }
    }
}