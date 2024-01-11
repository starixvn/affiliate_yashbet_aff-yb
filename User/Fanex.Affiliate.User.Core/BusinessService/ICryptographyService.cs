namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ICryptographyService
    {
        string EncryptPassword(string privateKey, string cipherText);

        string EncryptPassword(string password);

        string RsaDecrypt(string privateKey, string cipherText);

        string RsaEncrypt(string publicKey, string cipherText);

        string ComputeSha256Hash(string rawString);
    }
}
