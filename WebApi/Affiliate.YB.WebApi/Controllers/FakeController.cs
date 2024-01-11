using Affiliate.YB.Models;
using Affiliate.YB.WebApi.Models.Fake;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affiliate.YB.WebApi.Controllers
{
    public class FakeController : ApiController
    {

        [HttpPost]
        public SetAccountResult SetCustActStatus(AccountParam param)
        {
            return new SetAccountResult
            {
                Data = false,
                ErrorCode = 99
            };
        }
      
    }
}
