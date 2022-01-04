namespace Nomad.Sorter.Domain.Identitifiers;

public static class IdentifierExtensions
{
    public static BayId ToBayId(this string value) =>
        new(value);
    
    public static ParcelId ToParcelId(this string value) =>
        new(value);
}