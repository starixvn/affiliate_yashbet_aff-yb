using Fanex.Affiliate.User.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Extensions
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void FormatNumber_WhenCall_ReturnCorrect()
        {
            var list = new List<int> { 1, 2, 3 };
            var appendList = list.Append(new int[] { 4, 5 });

            Assert.Equal(5, appendList.Count());
            Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, appendList.ToList());
        }
    }
}
