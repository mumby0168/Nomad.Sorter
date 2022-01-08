using Nomad.Sorter.Domain.Entities.Abstractions;

namespace Nomad.Sorter.Application.Infrastructure;

public interface IParcelRepository
{
    ValueTask CreateParcel(IParcel parcel, CancellationToken cancellationToken = default);
}