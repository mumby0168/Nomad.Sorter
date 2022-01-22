using MassTransit;
using Nomad.Simulator.Services;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Simulator.Workers;

public class MessagingWorker : BackgroundService
{
    private readonly Queue<object> _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessagingWorker> _logger;

    public MessagingWorker(
        Queue<object> queue,
        IServiceProvider serviceProvider,
        ILogger<MessagingWorker> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            while (_queue.TryDequeue(out var operation))
            {
                _logger.LogInformation("Operation de-queued");
                switch (operation)
                {
                    case ParcelPreAdviceCommand preAdviceCommand:
                        _logger.LogInformation("parcel pre-advice for parcel {ParcelId}",
                            preAdviceCommand.ParcelId);

                        await Send(preAdviceCommand);
                        break;
                    case ParcelInductedEvent inductedEvent:
                        _logger.LogInformation("parcel inducted event for parcel {ParcelId}",
                            inductedEvent.ParcelId);
                    
                        await Publish(inductedEvent);
                        break;
                    case VehicleDockedEvent dockedEvent:
                        _logger.LogInformation("vehicle docking event with capacity {Capacity}",
                            dockedEvent.ParcelCapacity);
                    
                        await Publish(dockedEvent);
                        break;
                }
            }
            
            await Task.Delay(500, stoppingToken);
        }
    }

    private Task Publish<T>(T e)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        return sender.Publish(e);
    }

    private Task Send<T>(T command)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
        return sender.Send(command);
    }
}