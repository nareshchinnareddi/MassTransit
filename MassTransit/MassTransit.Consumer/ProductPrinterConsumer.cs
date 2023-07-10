using MassTransit.SharedModels;
using Newtonsoft.Json;

namespace MassTransit.Consumer;
class ProductPrinterConsumer : IConsumer<Product>
{
    public async Task Consume(ConsumeContext<Product> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"ProductPrinter message: {jsonMessage}");
    }
}