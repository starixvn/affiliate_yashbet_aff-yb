namespace Affiliate.YB.WebApi.ConsoleTest
{
    using Newtonsoft.Json;
    using Refit;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Program
    {
        private static async Task Main(string[] args)
        {
            var affiliateApi = RestService.For<IAffiliateApi>("http://localhost:57842/api/");

            var affiliates = await affiliateApi.GetAffiliates();

            Console.WriteLine(JsonConvert.SerializeObject(affiliates));
            Console.ReadLine();
        }
    }

    public interface IAffiliateApi
    {
        [Get("/test/TestMySql")]
        Task<IEnumerable<Affiliate1>> GetAffiliates();
    }

    public class Affiliate1
    {
        public Affiliate1(string affiliateId, int custId, int siteId)
        {
            AffiliateId = affiliateId;
            CustId = custId;
            SiteId = siteId;
        }

        public string AffiliateId { get; }

        public int CustId { get; }

        public int SiteId { get; }
    }
}