using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ASBDemo.Processor;

public class ConsoleApp(ILogger<ConsoleApp> logger, IConfiguration config, IServiceHandler handler) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var client = new ServiceBusClient(config["ServiceBus:ConnectionString"]);
        var processor = client.CreateProcessor(config["ServiceBus:Queue"], new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += handler.MessageHandler;
        processor.ProcessErrorAsync += handler.ErrorHandler;

        var cancellationTokenSource = new CancellationTokenSource();
        AddConsoleCancellation(cancellationTokenSource);

        try
        {
            logger.LogInformation("Starting message processing");
            await processor.StartProcessingAsync(cancellationTokenSource.Token);
            await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            logger.LogError("Stopping message processing");
        }
        finally
        {
            await processor.StopProcessingAsync(cancellationToken);
            logger.LogInformation("Message processing stopped");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopped");
        return Task.CompletedTask;
    }

    private void AddConsoleCancellation(CancellationTokenSource source)
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            source.Cancel();
            logger.LogInformation("Cancellation requested, shutting down...");
        };
    }
}