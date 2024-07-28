using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using ASBDemo.Processor.Events;

namespace ASBDemo.Processor;

public class ServiceHandler(ILogger<ServiceHandler> logger) : IServiceHandler
{
    public async Task MessageHandler(ProcessMessageEventArgs args)
    {
        logger.LogInformation($"Received message: {args.Message.Body}");
        try
        {
            // This assumes that the message body is a JSON-serialized RocketLaunchedEvent
            var msg = args.Message.Body.ToObjectFromJson<RocketLaunchedEvent>();
            logger.LogInformation($"Rocket {msg.RocketName} launched from {msg.LaunchLocation} at {msg.LaunchDateTime}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deserializing message");
            await args.AbandonMessageAsync(args.Message);
            return;
        }

        await args.CompleteMessageAsync(args.Message);
    }

    public Task ErrorHandler(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, $"Error processing message: {args.Exception.Message}");

        return Task.CompletedTask;
    }
}