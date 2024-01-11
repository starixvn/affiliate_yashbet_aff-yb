using System.Text;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class ConverterUtils
    {
        public static string Base64Encode(string plainText, Encoding encoding = null)
        {
            var encoder = encoding ?? new ASCIIEncoding();
            var plainTextBytes = encoder.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData, Encoding encoding = null)
        {
            var encoder = encoding ?? new ASCIIEncoding();
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return encoder.GetString(base64EncodedBytes);
        }

        public static string ToHex(byte[] buff)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < buff.Length; i++)
            {
                builder.Append(buff[i].ToString("X2")); // hex format
            }

            return builder.ToString();
        }
    }
}
