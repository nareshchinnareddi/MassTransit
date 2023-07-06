using MassTransit.SharedModels;
using Newtonsoft.Json;

namespace MassTransit.Consumer;
class OrderBillingConsumer : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"OrderBilling message: {jsonMessage}");
    }
}