using System.Collections.Generic;
using System.Reflection;
using DemoApps.Controllers.Settings;
using DemoApps.Infrastructure;
using DemoApps.Shared.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppAlpha
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UserStore>();
            services.AddSingleton<UserManager>();
            services.AddSingleton<PortunusSSOService>();
            services.AddSingleton(new AvailableSSOTargets
            {
                Targets = new List<(string dispName, string appName)>
                {
                     ("App Beta", "beta")
                }
            });
            services.AddSingleton(new SSOSettings
            {
                CurrentAppName = "alpha"
            });
            services.AddHttpClient();

            services
                .AddMvc()
                .AddApplicationPart(Assembly.Load(new AssemblyName("DemoApps.Controllers")));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AppAlpha_Cookies";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}