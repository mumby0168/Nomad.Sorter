using System.Diagnostics;
using CleanArchitecture.Exceptions;

namespace Nomad.Sorter.Domain.Identitifiers;

public record ClientId
{
    public ClientId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException<ClientId>("A client ID must be provided");
        }

        if (value.Length < 6)
        {
            throw new DomainException<ClientId>("A client ID must be at least 6 characters");
        }

        if (value.All(char.IsLetterOrDigit) is false)
        {
            throw new DomainException<ClientId>("A client ID must be alphanumeric");
        }
        
        Value = value;
    }

    public string Value { get; }
    
    public static implicit operator string(ClientId parcelId) => parcelId.Value;

    public override string ToString() => Value;
}