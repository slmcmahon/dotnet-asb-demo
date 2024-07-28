using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ASBDemo.Processor;

public class ServiceHandler(ILogger<ServiceHandler> logger) : IServiceHandler
{
    public async Task MessageHandler(ProcessMessageEventArgs args)
    {
        logger.LogInformation($"Received message: {args.Message.Body}");

        Console.WriteLine($"Received message: {args.Message.Body}");

        await args.CompleteMessageAsync(args.Message);
    }

    public Task ErrorHandler(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, $"Error processing message: {args.Exception.Message}");

        return Task.CompletedTask;
    }
}