using Fanex.Affiliate.User.Core.DTO.VerificationAccount;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public interface IHttpRequestParserHandler
    {
        QueryStringResult TryParseQueryString(string encryptedQuery);
    }
}
