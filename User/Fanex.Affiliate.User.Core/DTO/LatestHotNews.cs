using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.DTO
{
    public class LatestHotNews
    {
        public IDictionary<string, string> HotNewsList { get; set; }

        public bool IsImportant { get; set; }
    }
}
