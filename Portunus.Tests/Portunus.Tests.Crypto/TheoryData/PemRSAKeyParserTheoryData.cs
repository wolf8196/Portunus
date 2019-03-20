using System.Diagnostics.CodeAnalysis;
using Portunus.Tests.Shared.TestData;
using Xunit;

namespace Portunus.Crypto.Tests.TheoryData
{
    [ExcludeFromCodeCoverage]
    public static class PemRSAKeyParserTheoryData
    {
        public static TheoryData<string> InvalidPrivateKeys
        {
            get
            {
                var data = new TheoryData<string>
                {
                    { null },
                    { string.Empty },
                    { "test" },
                    { RSAKeys.PublicKey512 },
                    { RSAKeys.PublicKey1024 },
                    { RSAKeys.PublicKey2048 },
                    { RSAKeys.PublicKey4096 },
                };

                return data;
            }
        }

        public static TheoryData<string> ValidPrivateKeys
        {
            get
            {
                var data = new TheoryData<string>
                {
                    { RSAKeys.PrivateKey512 },
                    { RSAKeys.PrivateKey1024 },
                    { RSAKeys.PrivateKey2048 },
                    { RSAKeys.PrivateKey4096 },
                    { "-----BEGIN RSA PRIVATE KEY-----\nMIIEpQIBAAKCAQEAzLAzHqnNmeQmdikXtHRP1C1Boy3eanHQwFcyimLlbrm5LKaw\nsNPbtH9ycPiSp3WOnInmCn2DnQt7UWo4LHSNLUQupGEQTNiYuicYZt3JInWACWbi\nB15vj5Pz2G3ERKoHwV8PsrrayCmy05nMi+mILJ6q/VupvhiaMOP0ZSiuuTsNUMHn\npu+wDMqghnXAMW9cPF2XyJgJKh6K9K7VCetlAy1an0QjLPKY4VIi5QCNOnzhuy/H\nnWeeHfAVkNFWlYG042tY7YJRTwX8MKqmTxiM/Bml9ER4CxfHaoVTSdfwPC3YdhjH\nd/qUQVs07VEc5WWqpBpbv+Nk+80HtdxWPRclDQIDAQABAoIBAQCfq3dFHSpwOIQ0\nOlvufajkF6WTGC3fFQfFcn/PadQVqrUjeqhsV+eUBrMMIyuri82CmSOk3UI02IcK\n/HPzYbvbKsvDmJ/xgiipVeP4IlxLECOfsezx2J3n/38BPqcS9Rv1oSUKxC30ZGrG\nG7xvC+4Y+HDkEIP58REewEOeQb0CagNOtZPcvfO6lbLAi939fkEnxVsT1CnTViAS\nZqGIMA2rRbFrluxiT2OZINOQ0or0zUBNBmBGFb3fbrRIyPnGa4NkuTq0K0puUIBB\nUNc0mnQa0unAtl2A+thdvbsGw/RIoe4UvBueGyxlP29KKP6/2IcbSMAHF0N4dRWt\n1Di/Jp8VAoGBAPSW+nqD/jVjEc5o3Y/wn2kHybhx63StE5s8zs+kqjgA52waHjJO\nTYls25JmAQIaNttgMIHW2c+n7Xz+fvx+3SyapEKs4RM2qo51QQZBZWsNuvSba1Q5\nD57Tr14xp3Wr3hrMRHPiATCfdD1rsLOGnvNYutgVb1HqLpw18AiZirJfAoGBANY8\nskvzMojd3P0Mh93DJvAGhVOISuUVCBM2yZUpoxLiCbKahMTFsDj9ZFarvrZKDOIl\nBckuUJ1bM7I821rq73777xrfVqwjOpTWOPHDoUrL0+5iv1wtsB6SOnWvdkNyqSv7\nIXc7vpkn47qxIbJM+mORx5qzho3x7GLxOAyfzRgTAoGAAxNC4vd14sX0G1xGLOEh\nn/hxGNiV1Um3zWPeVc4ENeANCNbrOkaxwuCTgiu2J4ic1/VFptxEsM+ztLaech5G\nOaFYH159FfjB/DiNML7xv37Usu6hUtXE1IJk2hJVwK0AheW7lplo2mum0n7gIeG3\nbefXsY/Tbnw7ScuD0RfdHpECgYEA0qeCBc5PfWbymduNaRAwJEm1JYrZYeHJO+Ne\nWY5EbBfYw0bOkBF2ksMNu29KqkGr413WD5i76c44yeSvJ/nknq8oz/qVZdOKEbmG\n6qqa2UoMzNVKJmBCUf0lAH0UQ4PmNnnL0UrswfoIIZV0dbbdabR5WXN3NfGQp0Vb\nAIbNCwMCgYEAt87Lm2JoTF/jqkMLo3YfWvpEFkufFHnEAa+aocWHb+hNM6oghBYx\nn66pyImr+i4i65tFl2Rrxig8Z9dZvbodksSKeUT6BqjQFM5mtF+6WoiLGrsO9VPi\n33zEOT2WPLX5TGnS4FeKptGilNyiayUVz/Fow+5WBlLV90vCb2sFuCs=\n-----END RSA PRIVATE KEY-----" }
                };

                return data;
            }
        }

        public static TheoryData<string> InvalidPublicKeys
        {
            get
            {
                var data = new TheoryData<string>
                {
                    { null },
                    { string.Empty },
                    { "test" },
                    { RSAKeys.PrivateKey512 },
                    { RSAKeys.PrivateKey1024 },
                    { RSAKeys.PrivateKey2048 },
                    { RSAKeys.PrivateKey4096 },
                };

                return data;
            }
        }

        public static TheoryData<string> ValidPublicKeys
        {
            get
            {
                var data = new TheoryData<string>
                {
                    { RSAKeys.PublicKey512 },
                    { RSAKeys.PublicKey1024 },
                    { RSAKeys.PublicKey2048 },
                    { RSAKeys.PublicKey4096 },
                    { "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAzLAzHqnNmeQmdikXtHRP\n1C1Boy3eanHQwFcyimLlbrm5LKawsNPbtH9ycPiSp3WOnInmCn2DnQt7UWo4LHSN\nLUQupGEQTNiYuicYZt3JInWACWbiB15vj5Pz2G3ERKoHwV8PsrrayCmy05nMi+mI\nLJ6q/VupvhiaMOP0ZSiuuTsNUMHnpu+wDMqghnXAMW9cPF2XyJgJKh6K9K7VCetl\nAy1an0QjLPKY4VIi5QCNOnzhuy/HnWeeHfAVkNFWlYG042tY7YJRTwX8MKqmTxiM\n/Bml9ER4CxfHaoVTSdfwPC3YdhjHd/qUQVs07VEc5WWqpBpbv+Nk+80HtdxWPRcl\nDQIDAQAB\n-----END PUBLIC KEY-----" }
                };

                return data;
            }
        }
    }
}