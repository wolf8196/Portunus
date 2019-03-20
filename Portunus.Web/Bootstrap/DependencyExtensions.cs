using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portunus.Crypto.AuthenticatedEncryption;
using Portunus.Crypto.Interfaces;
using Portunus.Crypto.KeyGeneration;
using Portunus.Crypto.RSAEncryption;
using Portunus.SSO;
using Portunus.SSO.Interfaces;
using Portunus.SSO.Settings;
using Portunus.TokenProvider;
using Portunus.TokenProvider.Interfaces;
using Portunus.Web.Middleware;

namespace Portunus.Web.Bootstrap
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddPortunusDependencies(
               this IServiceCollection services,
               IConfiguration configuration)
        {
            services.AddPortunusSettings(configuration);
            services.AddStaticDependencies();

            return services;
        }

        private static IServiceCollection AddPortunusSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SSOSettings>(
                configuration.GetSection("SSOSettings"));

            return services;
        }

        private static IServiceCollection AddStaticDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IRSAEncryptor, RSAEncryptor>();
            services.AddSingleton<IRSAKeyParser, PemRSAKeyParser>();
            services.AddSingleton<ISymmetricKeyGenerator, SymmetricKeyGenerator>();
            services.AddSingleton<IAEADEncryptor, Aes256CbcHmacSha512>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            services.AddSingleton<ISSOManager, SSOManager>();
            services.AddSingleton<ExceptionHandlingMiddleware>();

            return services;
        }
    }
}