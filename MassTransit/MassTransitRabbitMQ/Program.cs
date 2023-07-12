using MassTransit;
using MassTransit.SharedModels;
using MassTransit.Testing;
using MassTransitRabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var rabbitMqSettingsOne = builder.Configuration.GetSection("RabbitMqSettingsOne").Get<RabbitMqSettings>();
var rabbitMqSettingsTwo = builder.Configuration.GetSection("RabbitMqSettingsTwo").Get<RabbitMqSettings>();
builder.Services.Configure<EndpointConfig>(builder.Configuration.GetSection("Endpoints"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit<IBusOne>(mt =>
                        mt.UsingRabbitMq((cntxt, cfg) =>
                        {
                            cfg.Host(rabbitMqSettingsOne.Uri, rabbitMqSettingsOne.VHost, c =>
                            {
                                c.Username(rabbitMqSettingsOne.UserName);
                                c.Password(rabbitMqSettingsOne.Password);
                            });
                            cfg.ConfigureMessageTopologyOne();
                        }));
builder.Services.AddMassTransit<IBusTwo>(mt =>
                        mt.UsingRabbitMq((cntxt, cfg) =>
                        {
                            cfg.Host(rabbitMqSettingsTwo.Uri, rabbitMqSettingsTwo.VHost, c =>
                            {
                                c.Username(rabbitMqSettingsTwo.UserName);
                                c.Password(rabbitMqSettingsTwo.Password);
                            });
                            cfg.ConfigureMessageTopologyTwo();
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
