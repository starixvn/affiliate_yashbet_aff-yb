using Fanex.Affiliate.User.Core.Extensions;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Extensions
{
    public class NumberFormatterTest
    {
        [Fact]
        public void FormatNumber_WhenCall_ReturnCorrect()
        {
            var formattedNumber = 10.FormatNumber(2);

            Assert.Equal("10", formattedNumber);
        }

        [Theory]
        [InlineData(0.004, "$0.00")]
        [InlineData(-0.004, "$0.00")]
        [InlineData(-0.04, "-$0.04")]
        public void ToStringMoneyWithSymbol_WhenCall_ReturnCorrect(decimal number, string result)
        {
            var formattedNumber = number.ToStringMoneyWithSymbol(2, "$");

            Assert.Equal(result, formattedNumber);
        }

        [Theory]
        [InlineData(0.004, "$0.00")]
        [InlineData(-0.004, "$0.00")]
        [InlineData(-0.04, "$-0.04")]
        public void ToStringMoneyWithCurrencySymbol_WhenCall_ReturnCorrect(decimal number, string result)
        {
            var formattedNumber = number.ToStringMoneyWithCurrencySymbol("$");

            Assert.Equal(result, formattedNumber);
        }

        [Fact]
        public void FormatDecimal_WhenCall_ReturnCorrect()
        {
            var formattedNumber = 10m.FormatDecimal(2);

            Assert.Equal("10.00", formattedNumber);
        }
    }
}
