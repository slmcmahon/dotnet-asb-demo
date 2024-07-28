using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ASBDemo.Processor;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).RunConsoleAsync();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                config.AddUserSecrets<Program>();
            }
            Console.WriteLine($"Environment: {hostingContext.HostingEnvironment.EnvironmentName}");
        })
        .ConfigureServices((hostingContext, services) =>
        {
            services.AddSingleton<IHostedService, ConsoleApp>();
            services.AddSingleton<IServiceHandler, ServiceHandler>();
            services.AddSingleton(hostingContext.Configuration);
        });
}
