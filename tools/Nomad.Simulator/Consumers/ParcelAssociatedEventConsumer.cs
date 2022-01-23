using MassTransit;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Events.Outbound;

namespace Nomad.Simulator.Consumers;

public class ParcelAssociatedEventConsumer : IConsumer<ParcelAssociatedEvent>
{
    private readonly Queue<object> _queue;

    public ParcelAssociatedEventConsumer(Queue<object> queue) => 
        _queue = queue;

    public Task Consume(ConsumeContext<ParcelAssociatedEvent> context)
    {
        Task.Run(() =>
        {
            Task.Delay(5000).Wait();
            _queue.Enqueue(new ParcelLoadedEvent(context.Message.ParcelId, context.Message.BayId));
        });

        return Task.CompletedTask;
    }
}