using Convey.CQRS.Commands;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Domain.Factories;

namespace Nomad.Sorter.Application.Handlers.Commands;

public class CreateBayCommandHandler : ICommandHandler<CreateBayCommand>
{
    private readonly IBayRepository _bayRepository;
    private readonly IBayFactory _bayFactory;

    public CreateBayCommandHandler(
        IBayRepository bayRepository, 
        IBayFactory bayFactory)
    {
        _bayRepository = bayRepository;
        _bayFactory = bayFactory;
    }
    
    public async Task HandleAsync(CreateBayCommand command, CancellationToken cancellationToken)
    {
        var bay = _bayFactory.Create(command.BayId);
        await _bayRepository.SaveBay(bay, cancellationToken);
    }
}