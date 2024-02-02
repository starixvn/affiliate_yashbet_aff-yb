using Fanex.Affiliate.Admin.Core.Apis;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using Fanex.Affiliate.Admin.Core.MetaModels.Api;
using Fanex.Affiliate.Admin.Core.Utilities;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly IAppSetting _appSetting;
        private readonly ITokenApi _tokenApi;

        public AuthenticatedHttpClientHandler(IAppSetting appSetting)
        {
            _appSetting = appSetting;
            base.Proxy = appSetting.IsUseMemberSiteProxy ? new WebProxy(new Uri(appSetting.MemberSiteProxyUrl)) : null;
            _tokenApi = TokenApiInstance(base.Proxy, appSetting.MemberSiteApiUrl);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SetJwtToken(request);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task SetJwtToken(HttpRequestMessage request)
        {
            var tokenInfo = await GetJwtToken();

            // Use the UriBuilder to parse and modify the url
            var uriBuilder = new UriBuilder(request.RequestUri);

            uriBuilder.Query = string.IsNullOrEmpty(uriBuilder.Query)
                ? $"access_token={tokenInfo.AccessToken}"
                : $"{uriBuilder.Query.Trim('?')}&access_token={tokenInfo.AccessToken}";

            request.RequestUri = uriBuilder.Uri;
        }

        private async Task<TokenInfo> GetJwtToken()
        {
            var tokenRequestParam = new TokenRequestParam
            {
                Username = $"{_appSetting.SiteName}_{_appSetting.MemberSiteApiUsername}",
                Password = _appSetting.MemberSiteApiPassword
            };

            try
            {
                var token = await _tokenApi.GetToken(tokenRequestParam);
                return token;

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);

                throw new InvalidOperationException($"Can't get JwtSecurityToken for {tokenRequestParam.Username}!");
            }
        }

        private static ITokenApi TokenApiInstance(IWebProxy proxy, string tokenApiUrl)
        {
            var handler = new HttpClientHandler
            {
                Proxy = proxy
            };

            return RestService.For<ITokenApi>(
                new HttpClient(handler)
                {
                    BaseAddress = new Uri(tokenApiUrl)
                });
        }
    }
}
