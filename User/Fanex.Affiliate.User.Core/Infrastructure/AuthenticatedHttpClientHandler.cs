using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly string machineIP;
        private readonly string secretKey;

        public AuthenticatedHttpClientHandler(string secretKey)
        {
            this.secretKey = secretKey;
            machineIP = GetLocalIPAddress();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Authenticate(request));

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public string Authenticate(HttpRequestMessage request)
        {
            var requestUri = HttpUtility.UrlEncode(request.RequestUri.PathAndQuery.ToLowerInvariant().Replace("api//", "api/"));
            string requestHttpMethod = request.Method.ToString();

            //Calculate UNIX time
            DateTime epochStart = DateTimeHelper.CreateDateTime(1970, 01, 01);
            TimeSpan timeSpan = DateTimeHelper.UtcNowDateTimeOffset.DateTime - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Creating the raw signature string
            string signatureRawData = $"{machineIP}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}";

            var secretKeyByteArray = Convert.FromBase64String(secretKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                // Setting the values in the Authorization header using custom scheme (amx)
                var authHeader = $"amx {machineIP}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}";

                return authHeader;
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString().Replace(":", "_");
                }
            }

            throw new InvalidOperationException("Local IP Address Not Found!");
        }
    }
}