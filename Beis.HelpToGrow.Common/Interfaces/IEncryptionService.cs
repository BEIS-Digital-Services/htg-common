namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface IEncryptionService
    {
        public string Encrypt(string plainText, string vendorSalt);
        public string Decrypt(string cipherText, string vendorSalt);
    }
}