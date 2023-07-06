using MassTransit;
using MassTransit.Consumer;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("order_archive", e =>
    {
        e.Consumer<OrderArchiveConsumer>();
    });
    cfg.ReceiveEndpoint("order_billing", e =>
    {
        e.Consumer<OrderBillingConsumer>();
    });
    cfg.ReceiveEndpoint("order_printer", e =>
    {
        e.Consumer<OrderPrinterConsumer>();
    });
    cfg.ReceiveEndpoint("order_tracking", e =>
    {
        e.Consumer<OrderTrackingConsumer>();
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