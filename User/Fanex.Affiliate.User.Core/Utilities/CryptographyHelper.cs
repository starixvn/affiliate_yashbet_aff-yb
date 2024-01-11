using Fanex.Affiliate.User.Core.Constants;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class CryptographyHelper
    {
        public static string Encrypt(string inputString)
        {
            return CfsEnCode(inputString);
        }

        public static string SHA256Hash(string input, string secret)
        {
            var secretKey = secret ?? string.Empty;
            var encoding = new ASCIIEncoding();
            var keyByte = encoding.GetBytes(secretKey);
            var messageBytes = encoding.GetBytes(input);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);

                return ConverterUtils.ToHex(hashmessage).ToLowerInvariant();
            }
        }

        private static string CfsEnCode(string codeStr)
        {
            const int Num = 30;
            int num2 = Num - codeStr.Length;

            var codeBuilder = new StringBuilder();
            codeBuilder.Append(codeStr);
            if (num2 > 1)
            {
                const int SectionSignNumber = 21;
                for (int i = 1; i <= num2; i++)
                {
                    codeBuilder.Append(Convert.ToChar(SectionSignNumber));
                }
            }

            var codeString = codeBuilder.ToString();
            double num3 = 1.0;
            for (int j = 1; j <= Num; j++)
            {
                int num4 = Num + Convert.ToInt32(Convert.ToChar(codeString.Substring(j - 1, 1))) * j;
                num3 *= num4;
            }
            var num3String = num3.ToString(CultureInfo.InvariantCulture);
            var textBuilder = new StringBuilder();
            for (int k = 1; k <= num3String.Length; k++)
            {
                var length = k + 3 > num3String.Length ? num3String.Length - k + 1 : 3;
                textBuilder.Append(CfsCode(num3String.Substring(k - 1, length)));
            }

            var text = textBuilder.ToString();
            var text2 = new StringBuilder();
            for (int l = 20; l <= text.Length - Number.Eighteen; l += Number.Two)
            {
                text2.Append(text.Substring(l - 1, 1));
            }
            return text2.ToString();
        }

        private static string CfsCode(string nWord)
        {
            var text = new StringBuilder();
            for (int i = 1; i <= nWord.Length; i++)
            {
                text.Append(Convert.ToInt32(Convert.ToChar(nWord.Substring(i - 1, 1))));
            }

            return Convert.ToInt64(text.ToString()).ToString("X");
        }
    }
}