using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class CryptographyServiceTest
    {
        private const int KEY_SIZE = 2048;

        [Fact]
        public void EncryptPassword_WhenCall_ReturnCorrectHashingPassword()
        {
            var password = "test";
            var keyPair = GetXmlKeyPair();
            var encryptPass = RsaEncrypt(keyPair, password);
            var base64EncodePrivateKey = ConvertToBase64(keyPair);
            var service = CreateService();

            var hashingPass = service.EncryptPassword(base64EncodePrivateKey, encryptPass);

            Assert.Equal(CryptographyHelper.Encrypt(password), hashingPass);
        }

        [Fact]
        public void EncryptPassword_WhenCall_ThrowException()
        {
            var cipherText = "123abc";
            var keyPair = GetXmlKeyPair();
            var base64EncodePrivateKey = ConvertToBase64(keyPair);
            var service = CreateService();

            var hashingPass = service.EncryptPassword(base64EncodePrivateKey, cipherText);

            Assert.Equal(CryptographyHelper.Encrypt(cipherText), hashingPass);
        }

        [Fact]
        public void EncryptPassword_WhenCall_ReturnCorrect()
        {
            var password = "test";
            var service = CreateService();

            var hashingPass = service.EncryptPassword(password);

            Assert.Equal(CryptographyHelper.Encrypt(password), hashingPass);
        }

        [Fact]
        public void RsaDecrypt_WhenCall_ReturnCorrectHashingPassword()
        {
            var password = "test";
            var keyPair = GetXmlKeyPair();
            var encryptPass = RsaEncrypt(keyPair, password);
            var base64EncodePrivateKey = ConvertToBase64(keyPair);
            var service = CreateService();

            var decryptPass = service.RsaDecrypt(base64EncodePrivateKey, encryptPass);

            Assert.Equal(password, decryptPass);
        }

        [Fact]
        public void RsaEncrypt_WhenCallWithCorrectKey_NotThrowException()
        {
            var text = "test";
            var publicKey = GetXmlKeyPair();
            var base64Key = ConvertToBase64(publicKey);
            var service = CreateService();

            service.RsaEncrypt(base64Key, text);

            Assert.True(true);
        }

        [Fact]
        public void ComputeSha256Hash_WhenCallWithNotNullString_NotThrowException()
        {
            var service = CreateService();

            service.ComputeSha256Hash("abc");

            Assert.True(true);
        }

        private static string GetXmlKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                return rsa.ToXmlString(true);
            }
        }

        private static string RsaEncrypt(string key, string plainText)
        {
            using (var rsa = new RSACryptoServiceProvider(KEY_SIZE))
            {
                rsa.PersistKeyInCsp = true;
                rsa.FromXmlString(key);
                var encryptText = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                return Convert.ToBase64String(encryptText);
            }
        }

        private static string ConvertToBase64(string plantText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plantText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static CryptographyService CreateService()
            => new CryptographyService();
    }
}
