using MassTransit;
using MassTransit.SharedModels;
using MassTransit.Testing;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
builder.Services.Configure<EndpointConfig>(builder.Configuration.GetSection("Endpoints"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(mt =>
                        mt.UsingRabbitMq((cntxt, cfg) =>
                        {
                            cfg.Host(rabbitMqSettings.Uri, "/", c =>
                            {
                                c.Username(rabbitMqSettings.UserName);
                                c.Password(rabbitMqSettings.Password);
                            });
                            cfg.Message<Order>(x => { x.SetEntityName("order"); });
                            cfg.Publish<Order>(x => { x.ExchangeType = ExchangeType.Direct; x.Exclude = true; });
                        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
