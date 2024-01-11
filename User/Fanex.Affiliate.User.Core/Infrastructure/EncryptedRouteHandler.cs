using System.Web;
using System.Web.Routing;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class EncryptedRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new EncryptedHandler(requestContext);
        }
    }
}
