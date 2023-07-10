using MassTransit;
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
    private readonly IPublishEndpoint _publishEndpoint;
    public OrdersController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Order(OrderDto orderDto)
    {
        await _publishEndpoint.Publish<Order>(new
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
        await _publishEndpoint.Publish<Product>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        }, x => { x.SetRoutingKey(orderDto.ProductName); });
        return Ok();
    }
}