namespace Portunus.Crypto.Interfaces
{
    public interface IAEADEncryptor
    {
        int KeySize { get; }

        byte[] Encrypt(byte[] plainText, byte[] key, byte[] associatedData = null);

        byte[] Decrypt(byte[] cipherText, byte[] key, byte[] associatedData = null);
    }
}