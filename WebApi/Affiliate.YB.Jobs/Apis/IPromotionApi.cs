using Affiliate.YB.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.Apis
{
    public interface IPromotionApi
    {
        [Get("/Promotion/GetPromotions")]
        Task<IEnumerable<PromotionModel>> GetPromotionsAsync(DateTime fromDate, DateTime toDate);

        [Get("/Promotion/GetPromotionSpecifics")]
        Task<IEnumerable<PromotionSpecificModel>> GetPromtionSpecificsAsync(DateTime fromDate, DateTime toDate);
    }
}
