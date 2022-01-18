using Microsoft.Extensions.Logging;
using Nomad.Sorter.Domain.Entities.Abstractions;

namespace Nomad.Sorter.Application.Logging;

public static class LoggerExtensions
{
    public static void LogParcelAssociated(this ILogger logger, string parcelId, string bayId) =>
        LoggerMessageDefinitions.ParcelAssociated(logger, parcelId, bayId, null!);

    public static void LogVehicleDocked(this ILogger logger, string vehicleRegistration, string bayId) =>
        LoggerMessageDefinitions.VehicleDocked(logger, vehicleRegistration, bayId, null!);
}