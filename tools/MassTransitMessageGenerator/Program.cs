using MassTransit;
using MassTransitMessageGenerator.Models;
using MassTransitMessageGenerator.Services;
using MassTransitMessageGenerator.Workers;
using Microsoft.AspNetCore.Mvc;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSwaggerGen();
services.AddEndpointsApiExplorer();
services.AddHostedService<MessagingWorker>();
services.AddSingleton<IMessageQueuingService, MessageQueuingService>();
services.AddSingleton<ISimulationService, SimulationService>();


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

app.MapGet("/newSimulation", ([FromServices] ISimulationService simulationService, [FromQuery] int parcels, [FromQuery] int vehicleSize) =>
{
    if (simulationService.IsSimulationInProgress)
    {
        return Results.Ok($"Simulation {simulationService.SimulationId} already in progress");
    }

    simulationService.TryStart(new StartSimulationRequest(parcels, vehicleSize));

    return Results.Ok($"Simulation started {simulationService.SimulationId}");
});

app.Run();