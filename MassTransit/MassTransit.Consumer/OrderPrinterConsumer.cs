using MassTransit.SharedModels;
using Newtonsoft.Json;

namespace MassTransit.Consumer;
class OrderPrinterConsumer : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"OrderPrinter message: {jsonMessage}");
    }
}