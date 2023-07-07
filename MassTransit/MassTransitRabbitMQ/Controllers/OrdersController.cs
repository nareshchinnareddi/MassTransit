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
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IBus _bus;
    private readonly IOptions<EndpointConfig> _endpointConfig;
    public OrdersController(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, IBus bus, IOptions<EndpointConfig> endpointConfig)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
        _bus = bus;
        _endpointConfig = endpointConfig;
    }

    [HttpPost("order_printer")]
    public async Task<IActionResult> OrderPrinter(OrderDto orderDto)
    {
        await _publishEndpoint.Publish<Order>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        }, x => { x.SetRoutingKey("printer"); });

        return Ok();
    }

    [HttpPost("order_archive")]
    public async Task<IActionResult> OrderArchive(OrderDto orderDto)
    {
        await _publishEndpoint.Publish<Order>(new
        {
            Id = 2,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        }, x => { x.SetRoutingKey("archive"); });
        return Ok();
    }
}