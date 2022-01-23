using MassTransit;
using Nomad.Simulator.Services;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Events.Outbound;

namespace Nomad.Simulator.Consumers;

public class ParcelAssociatedEventConsumer : IConsumer<ParcelAssociatedEvent>
{
    private readonly Queue<object> _queue;
    private readonly ISimulationService _simulationService;

    public ParcelAssociatedEventConsumer(Queue<object> queue, 
        ISimulationService simulationService)
    {
        _queue = queue;
        _simulationService = simulationService;
    }

    public Task Consume(ConsumeContext<ParcelAssociatedEvent> context)
    {
        Task.Run(() =>
        {
            Task.Delay(_simulationService.GetParcelAssociationDelay()).Wait();
            _queue.Enqueue(new ParcelLoadedEvent(context.Message.ParcelId, context.Message.BayId));
        });

        return Task.CompletedTask;
    }
}