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

var rabbitMqSettingsOne = Configuration.GetSection("RabbitMqSettingsOne").Get<RabbitMqSettings>();
var rabbitMqSettingsTwo = Configuration.GetSection("RabbitMqSettingsTwo").Get<RabbitMqSettings>();
var busOneControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMqSettingsOne.Uri, rabbitMqSettingsOne.VHost, c =>
    {
        c.Username(rabbitMqSettingsOne.UserName);
        c.Password(rabbitMqSettingsOne.Password);
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
});
var busTwoControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMqSettingsTwo.Uri, rabbitMqSettingsTwo.VHost, c =>
    {
        c.Username(rabbitMqSettingsTwo.UserName);
        c.Password(rabbitMqSettingsTwo.Password);
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

await busOneControl.StartAsync(new CancellationToken());
await busTwoControl.StartAsync(new CancellationToken());
try
{
    Console.WriteLine("Press enter to exit");
    await Task.Run(() => Console.ReadLine());
}
finally
{
    await busOneControl.StopAsync();
    await busTwoControl.StopAsync();
}