using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppAlpha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config
                        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logBuilder) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    if (env.IsDevelopment())
                    {
                        logBuilder.AddConsole();
                    }
                })
                .Build();

            host.Run();
        }
    }
}