using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Extensions;

public static class IdentifierExtensions
{
    public static BayId ToBayId(this string value) =>
        new(value);
    
    public static ParcelId ToParcelId(this string value) =>
        new(value);

    public static ClientId ToClientId(this string value) =>
        new(value);
}