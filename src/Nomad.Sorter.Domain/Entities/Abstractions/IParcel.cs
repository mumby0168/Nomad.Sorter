using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

public interface IParcel
{
    ParcelId ParcelId { get; }
}