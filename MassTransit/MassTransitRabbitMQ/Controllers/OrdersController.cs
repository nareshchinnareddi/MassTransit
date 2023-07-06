using MassTransit;
using MassTransit.SharedModels;
using MassTransitRabbitMQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MassTransitRabbitMQ.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;
    private readonly IOptions<EndpointConfig> _endpointConfig;
    public OrdersController(IPublishEndpoint publishEndpoint, IBus bus, IOptions<EndpointConfig> endpointConfig)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
        _endpointConfig = endpointConfig;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto orderDto)
    {
        await _publishEndpoint.Publish<Order>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        });
        return Ok();
    }
}