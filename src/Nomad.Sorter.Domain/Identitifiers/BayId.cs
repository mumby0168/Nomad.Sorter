using CleanArchitecture.Exceptions;
using Nomad.Sorter.Domain.Entities;

namespace Nomad.Sorter.Domain.Identitifiers;

public record BayId
{
    public BayId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException<BayId>("A bay ID cannot be null or empty");
        }

        if (value.Length is not 6)
        {
            throw new DomainException<BayId>("A bay ID must be 6 characters");
        }

        if (value[..3].Any(x => char.IsLetter(x) is false))
        {
            throw new DomainException<BayId>("The first 3 characters of a bay id must be letters");
        }
        
        if (value[3..6].Any(x => char.IsNumber(x) is false))
        {
            throw new DomainException<BayId>("The last 3 characters of a bay id must be numbers");
        }

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(BayId bayId) => bayId.Value;

    public override string ToString() => Value;
};