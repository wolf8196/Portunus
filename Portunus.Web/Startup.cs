using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portunus.Web.Bootstrap;

namespace Portunus.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters();

            services.AddPortunusDependencies(configuration);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UsePortunusExceptionHandling();
            }

            app.UseMvc();
        }
    }
}