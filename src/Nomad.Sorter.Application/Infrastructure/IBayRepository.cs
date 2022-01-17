using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Application.Infrastructure;

public interface IBayRepository
{
    ValueTask<IBay> GetBay(BayId bayId, CancellationToken cancellationToken = default);

    ValueTask SaveBay(IBay bay, CancellationToken cancellationToken = default);
}