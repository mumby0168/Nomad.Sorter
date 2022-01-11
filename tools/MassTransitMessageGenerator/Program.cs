using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSwaggerGen();
services.AddEndpointsApiExplorer();


EndpointConvention.Map<ParcelPreAdviceCommand>(new Uri($"queue:{ServiceBusConstants.Queues.ParcelPreAdviceQueue}"));
services.AddMassTransit(massTransit =>
{
    massTransit.UsingAzureServiceBus((registration, cfg) =>
    {
        cfg.Message<ParcelPreAdviceCommand>(x => { x.SetEntityName(ServiceBusConstants.Queues.ParcelPreAdviceQueue); });
        cfg.Message<ParcelInductedEvent>(x => x.SetEntityName(ServiceBusConstants.Topics.ParcelInductedTopic));
        cfg.Host(builder.Configuration.GetConnectionString("ServiceBus"));
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("api/commands/parcel-pre-advice",
    (ParcelPreAdviceCommand command, ISendEndpointProvider sendEndpointProvider) => sendEndpointProvider.Send(command));

app.MapPost("api/commands/parcel-pre-advice/test", async (ISendEndpointProvider sendEndpointProvider) =>
{
    var command = new ParcelPreAdviceCommand(Guid.NewGuid().ToString("N"),
        "ClientTest123",
        Guid.NewGuid().ToString(),
        "HU10TST");

    await sendEndpointProvider.Send(command);

    return Results.Ok(command);
});

app.MapPost("api/events/parcel-inducted",
    (ParcelInductedEvent @event, IPublishEndpoint publishEndpoint) => publishEndpoint.Publish(@event));

app.Run();