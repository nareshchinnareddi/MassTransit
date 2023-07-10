using MassTransit.SharedModels;
using Newtonsoft.Json;

namespace MassTransit.Consumer;
class ProductArchiveConsumer : IConsumer<Product>
{
    public async Task Consume(ConsumeContext<Product> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"ProductArchive message: {jsonMessage}");
    }
}