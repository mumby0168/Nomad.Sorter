using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Testing;

public static class NomadIdentifiers
{
    public static readonly ParcelId ParcelId = new(Guid.NewGuid().ToString("N"));

    public static readonly ClientId ClientId = new($"CLIENT1");
}