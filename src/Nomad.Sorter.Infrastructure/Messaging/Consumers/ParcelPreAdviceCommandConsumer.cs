using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Commands;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class ParcelPreAdviceCommandConsumer : IConsumer<ParcelPreAdviceCommand>
{
    private readonly ILogger<ParcelPreAdviceCommandConsumer> _logger;
    private readonly IMediator _mediator;

    public ParcelPreAdviceCommandConsumer(ILogger<ParcelPreAdviceCommandConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ParcelPreAdviceCommand> context)
    {
        try
        {
            _logger.LogInformation(
                "Processing parcel pre advice for parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);

            await _mediator.Send(context.Message, context.CancellationToken);

            _logger.LogInformation(
                "Successfully processed parcel pre advice for parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);

            throw;
        }
    }
}