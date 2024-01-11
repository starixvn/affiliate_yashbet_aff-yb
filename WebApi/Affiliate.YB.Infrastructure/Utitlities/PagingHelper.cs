using System.Collections.Generic;
using System.Linq;

namespace Affiliate.YB.Utitlities
{
    public static class PagingHelper
    {
        public static IEnumerable<IEnumerable<T>> PagingData<T>(IEnumerable<T> datas, int max)
        {
            var result = new List<IEnumerable<T>>();
            result.Add(datas.Take(max));
            var remain = datas.Skip(max);
            while (remain.Count() > 0)
            {
                result.Add(remain.Take(max));
                remain = remain.Skip(max);
            }
            return result;
        }
    }
}
