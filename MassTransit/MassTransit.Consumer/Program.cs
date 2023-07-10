using MassTransit;
using MassTransit.Consumer;
using MassTransit.SharedModels;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var rabbitMqSettings = Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMqSettings.Uri, rabbitMqSettings.VHost, c =>
    {
        c.Username(rabbitMqSettings.UserName);
        c.Password(rabbitMqSettings.Password);
    });
    cfg.ReceiveEndpoint("order_archive", e =>
    {
        e.Consumer<OrderArchiveConsumer>();
        e.Bind("order", x =>
        {
            x.RoutingKey = "archive";
            x.ExchangeType = ExchangeType.Direct;
        });
        e.ConfigureConsumeTopology = false;
    });
    cfg.ReceiveEndpoint("order_printer", e =>
    {
        e.Consumer<OrderPrinterConsumer>();
        e.Bind("order", x =>
        {
            x.RoutingKey = "printer";
            x.ExchangeType = ExchangeType.Direct;
        });
        e.ConfigureConsumeTopology = false;
    });
    cfg.ReceiveEndpoint("product_archive", e =>
    {
        e.Consumer<ProductArchiveConsumer>();
        e.Bind("MassTransit.SharedModels:Product", x =>
        {
            x.RoutingKey = "archive";
            x.ExchangeType = ExchangeType.Direct;
        });
        e.ConfigureConsumeTopology = false;
    });
    cfg.ReceiveEndpoint("product_printer", e =>
    {
        e.Consumer<ProductPrinterConsumer>();
        e.Bind("MassTransit.SharedModels:Product", x =>
        {
            x.RoutingKey = "printer";
            x.ExchangeType = ExchangeType.Direct;
        });
        e.ConfigureConsumeTopology = false;
    });
});

await busControl.StartAsync(new CancellationToken());
try
{
    Console.WriteLine("Press enter to exit");
    await Task.Run(() => Console.ReadLine());
}
finally
{
    await busControl.StopAsync();
}