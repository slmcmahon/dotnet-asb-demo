using Azure.Messaging.ServiceBus;

namespace ASBDemo.Processor;

public interface IServiceHandler
{
    Task MessageHandler(ProcessMessageEventArgs args);

    Task ErrorHandler(ProcessErrorEventArgs args);
}