using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

public interface IBay
{
    BayId BayId { get; }
    
    BayStatus Status { get; }
    
    DateTime LastUpdatedTimeUtc { get; }
    
    DateTime? CreatedTimeUtc { get; }
}