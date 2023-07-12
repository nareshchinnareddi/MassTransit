using MassTransit;
using MassTransit.DependencyInjection;
using MassTransit.SharedModels;
using MassTransit.Transports;
using MassTransitRabbitMQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MassTransitRabbitMQ.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly Bind<IBusOne, IPublishEndpoint> _publishEndpointOne;
    private readonly Bind<IBusTwo, IPublishEndpoint> _publishEndpointTwo;
    public OrdersController(Bind<IBusOne, IPublishEndpoint> publishEndpointOne, Bind<IBusTwo, IPublishEndpoint> publishEndpointTwo)
    {
        _publishEndpointOne = publishEndpointOne;
        _publishEndpointTwo = publishEndpointTwo;
    }

    [HttpPost]
    public async Task<IActionResult> Order(OrderDto orderDto)
    {
        await _publishEndpointOne.Value.Publish<Order>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        });
        //}, x => { x.SetRoutingKey(orderDto.ProductName); x.DestinationAddress = new Uri("exchange:order"); });
        return Ok();
    }

    [HttpPost("Product")]
    public async Task<IActionResult> Product(OrderDto orderDto)
    {
        await _publishEndpointTwo.Value.Publish<Product>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        }, x => { x.SetRoutingKey(orderDto.ProductName); });
        return Ok();
    }
}