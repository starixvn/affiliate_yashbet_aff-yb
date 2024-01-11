using Newtonsoft.Json;

namespace Fanex.Affiliate.Admin.Core.DTO
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [JsonIgnore]
        public bool IsP2P { get; set; }
    }
}
