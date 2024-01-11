namespace Fanex.Affiliate.User.Core.Extensions
{
    using System;
    using System.Globalization;

    public static class NumberFormatter
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        public static string FormatNumber(this int value, byte digit = 2)
        {
            return value.ToString("#,##0");
        }

        public static string ToStringMoneyWithSymbol(this decimal value, byte digit = 2, string symbol = "")
        {
            return IsShowNegative(value, digit) ? $"-{symbol}{Math.Abs(value).FormatDecimal(digit)}" : $"{symbol}{value.FormatDecimal(digit)}";
        }

        public static string ToStringMoneyWithCurrencySymbol(this decimal value, string currencySymbol = "", byte digit = 2)
        {
            return IsShowNegative(value, digit) ? $"{currencySymbol}-{Math.Abs(value).FormatDecimal(digit)}" : $"{currencySymbol}{value.FormatDecimal(digit)}";
        }

        public static string FormatDecimal(this decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }

        private static bool IsShowNegative(decimal value, byte digit)
        {
            const double Ten = 10;
            const double NegativeFive = -5;

            //-$0.00xy (x < 5), digit = 2, the amount must be shown as $0.00
            if ((double)value * (Math.Pow(Ten, (digit + 1))) > NegativeFive)
            {
                return false;
            }

            return true;
        }
    }
}