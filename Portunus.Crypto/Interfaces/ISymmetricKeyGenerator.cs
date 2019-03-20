namespace Portunus.Crypto.Interfaces
{
    public interface ISymmetricKeyGenerator
    {
        byte[] Generate(int length);
    }
}