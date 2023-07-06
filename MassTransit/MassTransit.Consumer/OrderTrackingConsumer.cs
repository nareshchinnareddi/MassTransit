using MassTransit.SharedModels;
using Newtonsoft.Json;

namespace MassTransit.Consumer;
class OrderTrackingConsumer : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"OrderTracking message: {jsonMessage}");
    }
}