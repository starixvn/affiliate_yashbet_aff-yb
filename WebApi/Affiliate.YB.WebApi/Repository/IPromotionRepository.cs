using Affiliate.YB.WebApi.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.WebApi.Repository
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetPromotionsAsync(DateTime fromDate, DateTime toDate);
    }
}