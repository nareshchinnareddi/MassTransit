using MassTransit;
using MassTransit.Consumer;
using MassTransit.SharedModels;
using Microsoft.Extensions.Configuration;

IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var rabbitMqSettings = Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMqSettings.Uri, "/", c =>
    {
        c.Username(rabbitMqSettings.UserName);
        c.Password(rabbitMqSettings.Password);
    });
    cfg.ReceiveEndpoint("order_archive", e =>
    {
        e.Consumer<OrderArchiveConsumer>();
    });
    cfg.ReceiveEndpoint("order_printer", e =>
    {
        e.Consumer<OrderPrinterConsumer>();
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