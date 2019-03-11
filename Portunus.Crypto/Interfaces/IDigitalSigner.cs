namespace Portunus.Crypto.Interfaces
{
    internal interface IDigitalSigner
    {
        int HashSize { get; }

        int KeySize { get; }

        byte[] Sign(byte[] message, byte[] key);

        bool Verify(byte[] message, byte[] key, byte[] signature);
    }
}