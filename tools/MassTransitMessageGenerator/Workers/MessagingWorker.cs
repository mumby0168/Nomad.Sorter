using Convey.CQRS.Commands;
using MassTransit;
using MassTransitMessageGenerator.Services;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;

namespace MassTransitMessageGenerator.Workers;

public class MessagingWorker : BackgroundService
{
    private readonly IMessageQueuingService _messageQueuingService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessagingWorker> _logger;

    public MessagingWorker(
        IMessageQueuingService messageQueuingService,
        IServiceProvider serviceProvider,
        ILogger<MessagingWorker> logger)
    {
        _messageQueuingService = messageQueuingService;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            while (_messageQueuingService.Operations.TryDequeue(out var operation))
            {
                _logger.LogInformation("Operation de-queued");
                if (operation is ParcelPreAdviceCommand preAdviceCommand)
                {
                    _logger.LogInformation("parcel pre-advice for parcel {ParcelId}",
                        preAdviceCommand.ParcelId);

                    await Send(preAdviceCommand);
                }
                else if (operation is ParcelInductedEvent inductedEvent)
                {
                    _logger.LogInformation("parcel inducted event for parcel {ParcelId}",
                        inductedEvent.ParcelId);
                    
                    await Publish(inductedEvent);
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