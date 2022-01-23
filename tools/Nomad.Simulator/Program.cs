using MassTransit;
using Nomad.Simulator.Consumers;
using Nomad.Simulator.Models;
using Nomad.Simulator.Services;
using Nomad.Simulator.Workers;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSwaggerGen();
services.AddEndpointsApiExplorer();
services.AddHostedService<MessagingWorker>();
services.AddSingleton(new Queue<object>());
services.AddSingleton<ISimulationService, SimulationService>();


EndpointConvention.Map<ParcelPreAdviceCommand>(new Uri($"queue:{ServiceBusConstants.Queues.ParcelPreAdviceQueue}"));
services.AddMassTransit(massTransit =>
{
    massTransit.AddConsumer<ParcelAssociatedEventConsumer>();

    massTransit.UsingAzureServiceBus((reg, cfg) =>
    {
        cfg.Message<ParcelPreAdviceCommand>(x =>
            x.SetEntityName(ServiceBusConstants.Queues.ParcelPreAdviceQueue));

        cfg.Message<ParcelInductedEvent>(x =>
            x.SetEntityName(ServiceBusConstants.Topics.ParcelInductedTopic));

        cfg.Message<VehicleDockedEvent>(x =>
            x.SetEntityName(ServiceBusConstants.Topics.VehicleDockedTopic));
        
        cfg.ExcludeInterfacesFromTopology();

        cfg.SubscriptionEndpoint(
            "nomad-simulator",
            ServiceBusConstants.Topics.ParcelAssociatedTopic,
            configurator =>
            {
                configurator.ConfigureConsumeTopology = false;
                configurator.PublishFaults = false;
                configurator.ConfigureConsumer<ParcelAssociatedEventConsumer>(reg);
            });

        cfg.Host(builder.Configuration.GetConnectionString("ServiceBus"));
    });
});

services.AddMassTransitHostedService();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("simulation/reset", (ISimulationService simulationService) => { simulationService.Cancel(); });

app.MapPost("/simulation/new", (StartSimulationCommand command, ISimulationService simulationService) =>
{
    if (simulationService.IsSimulationInProgress)
    {
        return Results.Conflict($"Simulation {simulationService.SimulationId} already in progress");
    }

    simulationService.TryStart(command);

    return Results.Ok(simulationService.Parcels.ToList());
});

app.MapPost("/simulation/dock", (SimulateVehicleDockingCommand command,
        ISimulationService simulationService) =>
    simulationService.DockVehicle(command)
        ? Results.Ok("Vehicle Docked")
        : Results.BadRequest("Simulation not in progress"));

app.Run();