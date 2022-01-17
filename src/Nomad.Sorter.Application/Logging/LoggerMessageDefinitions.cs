using Microsoft.Extensions.Logging;

namespace Nomad.Sorter.Application.Logging;

public static class LoggerMessageDefinitions
{
    //INFO
    internal static readonly Action<ILogger, string, string, Exception> ParcelAssociated =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            EventIds.ParcelAssociated,
            "Parcel {ParcelId} associated with vehicle docked at bay {BayId}");
}