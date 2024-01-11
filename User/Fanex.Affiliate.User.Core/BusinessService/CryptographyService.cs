using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class CryptographyService : ICryptographyService
    {
        private const int KeySize = 2048;

        public string EncryptPassword(string privateKey, string cipherText)
        {
            var plainPassword = Decrypt(DecodeUtfBase64(privateKey), cipherText);

            return CryptographyHelper.Encrypt(plainPassword);
        }

        public string EncryptPassword(string password)
        {
            return CryptographyHelper.Encrypt(password);
        }

        public string RsaDecrypt(string privateKey, string cipherText)
        {
            return Decrypt(DecodeUtfBase64(privateKey), cipherText);
        }

        public string RsaEncrypt(string publicKey, string cipherText)
        {
            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                rsa.PersistKeyInCsp = true;
                rsa.FromXmlString(DecodeUtfBase64(publicKey));

                var plainbytes = Encoding.UTF8.GetBytes(cipherText);
                var cipherbytes = rsa.Encrypt(plainbytes, false);

                return Convert.ToBase64String(cipherbytes);
            }
        }

        public string ComputeSha256Hash(string rawString)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));

                return ConverterUtils.ToHex(hashedBytes);
            }
        }

        private static string Decrypt(string privateKey, string cipherText)
        {
            const byte BitInByte = 8;

            try
            {
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = true;
                    rsa.FromXmlString(privateKey);

                    var bytes = Convert.FromBase64String(cipherText);

                    // Correct the error in the JS encryptor.
                    var rsaLength = rsa.KeySize / BitInByte;
                    if (bytes.Length < rsaLength)
                    {
                        byte[] tmp = new byte[rsaLength];
                        Buffer.BlockCopy(bytes, 0, tmp, tmp.Length - bytes.Length, bytes.Length);
                        bytes = tmp;
                    }

                    var plain = rsa.Decrypt(bytes, false);

                    return Encoding.UTF8.GetString(plain);
                }
            }
            catch
            {
                return cipherText;
            }
        }

        private static string DecodeUtfBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}