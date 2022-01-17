using Microsoft.Extensions.Logging;

namespace Nomad.Sorter.Application.Logging;

public static class EventIds
{
    //INFO 8000-8200
    public static EventId ParcelAssociated = new(
        8001,
        nameof(ParcelAssociated));
}