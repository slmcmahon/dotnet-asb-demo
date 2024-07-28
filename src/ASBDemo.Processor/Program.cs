using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace ASBDemo.Processor;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args);
        Log.Information("Starting Up");
        await host.RunConsoleAsync();
        Log.Information("Shutting Down");
    }
    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            hostingContext.HostingEnvironment.EnvironmentName = env;

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                config.AddUserSecrets<Program>();
            }

            Log.Information($"Environment: {hostingContext.HostingEnvironment.EnvironmentName}");
        })
        .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            })
        .ConfigureServices((hostingContext, services) =>
        {
            services.AddSingleton<IHostedService, ConsoleApp>();
            services.AddSingleton<IServiceHandler, ServiceHandler>();
            services.AddSingleton(hostingContext.Configuration);
        });
}
