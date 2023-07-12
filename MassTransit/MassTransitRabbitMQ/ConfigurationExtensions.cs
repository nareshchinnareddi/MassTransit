using MassTransit;
using MassTransit.SharedModels;
using RabbitMQ.Client;

namespace MassTransitRabbitMQ
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureMessageTopologyOne(this IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Message<Order>(x => { x.SetEntityName("order"); });
            configurator.Send<Order>(x =>
            {
                x.UseCorrelationId(context => context.Id);
                x.UseRoutingKeyFormatter(context => context.Message.ProductName);
            });

            configurator.Publish<Order>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
        public static void ConfigureMessageTopologyTwo(this IRabbitMqBusFactoryConfigurator configurator)
        {
            //configurator.Message<Product>(x => { x.SetEntityName("product"); });
            //configurator.Send<Product>(x =>
            //{
            //    x.UseCorrelationId(context => context.Id);
            //    x.UseRoutingKeyFormatter(context => context.Message.ProductName);
            //});

            configurator.Publish<Product>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}
