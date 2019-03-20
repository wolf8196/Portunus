using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Portunus.Crypto.Interfaces;

namespace Portunus.Crypto.RSAEncryption
{
    public class PemRSAKeyParser : IRSAKeyParser
    {
        public bool TryParsePrivate(string s, out RSAParameters key)
        {
            key = new RSAParameters();

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            try
            {
                var keyPair = (AsymmetricCipherKeyPair)ReadKey(s);

                var privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;

                key = new RSAParameters
                {
                    Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                    P = privateKeyParams.P.ToByteArrayUnsigned(),
                    Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                    DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                    DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                    InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned(),
                    D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                    Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned()
                };

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryParsePublic(string s, out RSAParameters key)
        {
            key = new RSAParameters();

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            try
            {
                var keyParams = (RsaKeyParameters)ReadKey(s);

                key = new RSAParameters
                {
                    Modulus = keyParams.Modulus.ToByteArrayUnsigned(),
                    Exponent = keyParams.Exponent.ToByteArrayUnsigned()
                };

                return true;
            }
            catch
            {
                return false;
            }
        }

        private object ReadKey(string s)
        {
            using (var stringReader = new StringReader(s))
            {
                var pemReader = new PemReader(stringReader);
                return pemReader.ReadObject();
            }
        }
    }
}