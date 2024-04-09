using Fanex.Affiliate.User.Core.DTO.Api;
using Fanex.Affiliate.User.Core.MetalModels.Api;
using Refit;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.Apis
{
    public interface ITokenApi
    {
        [Post("/api/Auth")]
        Task<TokenInfo> GetTokenAsync([Body] TokenRequestParam param);
    }
}
