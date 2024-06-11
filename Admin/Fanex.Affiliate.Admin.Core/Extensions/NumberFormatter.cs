using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Extensions
{
    public static class NumberFormatter
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        public static string FormatDecimal(this decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }
    }
}
