using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portunus.Crypto;
using Portunus.Crypto.Interfaces;
using Portunus.SSO;
using Portunus.SSO.Interfaces;
using Portunus.SSO.Settings;
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
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();
            services.AddSingleton<ISSOManager, SSOManager>();
            services.AddSingleton<ExceptionHandlingMiddleware>();

            return services;
        }
    }
}