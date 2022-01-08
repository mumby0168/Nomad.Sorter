using CleanArchitecture.Exceptions;

namespace Nomad.Sorter.Domain.Identitifiers;

public record ParcelId
{
    public ParcelId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException<ParcelId>("A parcel ID cannot be null or empty");
        }

        if (Guid.TryParseExact(value, "N", out var _) is false)
        {
            throw new DomainException<ParcelId>("A parcel ID must be a valid guid with no separators");
        }
        
        Value = value;
    }

    public static implicit operator string(ParcelId parcelId) => parcelId.Value;

    public override string ToString() => Value;

    public string Value { get; init; }
}