using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

public interface IBay
{
    /// <summary>
    /// The ID of the bay.
    /// </summary>
    BayId BayId { get; }
    
    /// <summary>
    /// The status of the bay.
    /// </summary>
    BayStatus Status { get; }
    
    /// <summary>
    /// The time the bay was last updated.
    /// </summary>
    DateTime LastUpdatedTimeUtc { get; }
    
    /// <summary>
    /// The time the bay was created.
    /// </summary>
    DateTime? CreatedTimeUtc { get; }
    
    /// <summary>
    /// The information about a docked vehicle in a bay.
    /// </summary>
    /// <remarks>null if there is not vehicle docked in the bay.</remarks>
    DockingInformation? DockingInformation { get; }
}