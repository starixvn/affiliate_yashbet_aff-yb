using Affiliate.YB.Models;
using Affiliate.YB.Params;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affiliate.YB.WebApi.Controllers
{
    public class Product9WController : ApiController
    {

        private readonly IDynamicRepository dynamicRepository;

        public Product9WController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TpReport9W>> GetTpReport([FromUri] BaseParam param)
            => await dynamicRepository.FetchAsync<TpReport9W>(new GetTpReport9WCriteria
            {
                FromDate = param.FromDate,
                ToDate = param.ToDate,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            });

        [HttpGet]
        public async Task<IEnumerable<TpWager9W>> GetTpWager([FromUri] BaseParam param)
            => await dynamicRepository.FetchAsync<TpWager9W>(new GetTpWager9WCriteria
            {
                FromDate = param.FromDate,
                ToDate = param.ToDate,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            });
    }
}