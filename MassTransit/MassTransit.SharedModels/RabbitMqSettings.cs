namespace MassTransit.SharedModels;
public class RabbitMqSettings
{
    public string Uri { get; set; } = null!;
    public string Port { get; set; } = null!;
    public string VHost { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}