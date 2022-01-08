using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities;

public class Bay : BaseEntity, IBay
{
    /// <inheritdoc cref="IBay"/>
    [JsonIgnore]
    public BayId BayId { get; }

    /// <inheritdoc cref="IBay"/>
    [JsonConverter(typeof(StringEnumConverter))]
    public BayStatus Status { get; }

    /// <inheritdoc cref="IBay"/>
    public DockingInformation? DockingInformation { get; }

    /// <summary>
    /// Creates an instance of a bay.
    /// </summary>
    /// <param name="bayId">The ID of the bay.</param>
    /// <remarks>This object should only be created via a factory in the domain layer.</remarks>
    internal Bay(BayId bayId) : base(nameof(Bay))
    {
        BayId = bayId;
        Status = BayStatus.Empty;
        Id = bayId;
        DockingInformation = null;
    }

    /// <summary>
    /// Creates a bay from when being deserialized via JSON.
    /// </summary>
    /// <param name="id">The ID of the bay.</param>
    /// <param name="status">The status of the bay.</param>
    /// <param name="dockingInformation">The optional docking information of the bay.</param>
    /// <remarks>This should only be used when being deserialized from JSON.
    /// This constructor uses strongly typed ID's so if the bay id for example is invalid this will throw an exception.</remarks>
    [JsonConstructor]
    private Bay(
        string id,
        BayStatus status,
        DockingInformation? dockingInformation = null) : base(nameof(Bay))
    {
        Status = status;
        BayId = id.ToBayId();
        Id = id;
        DockingInformation = dockingInformation;
    }
}